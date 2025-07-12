using Money.Domain;
using System.Linq;
using System.Collections.Generic;

namespace Money.Optimization;

public class RolloverSimulator
{
    private readonly SimulationParams p;
    private readonly Dictionary<TaxCategory, List<TaxBracket>> baseBrackets = new();
    private readonly Dictionary<TaxCategory, decimal> baseRates = new();
    private readonly Dictionary<TaxCategory, List<BracketChange>> bracketChanges = new();
    private readonly Dictionary<TaxCategory, List<RateChange>> rateChanges = new();

    public int Years => p.TargetAge - p.StartAge + 1;
    public SimulationParams Params => p;

    public class SimulationState
    {
        public List<Investment> Investments { get; set; } = new();
        public decimal Brokerage { get; set; }
        public decimal TotalTaxesPaid { get; set; }
        public decimal RolloverTaxBucket { get; set; }
        public Dictionary<TaxCategory, List<TaxBracket>> Brackets { get; set; } = new();
        public Dictionary<TaxCategory, decimal> Rates { get; set; } = new();
        public decimal LastExternalIncome { get; set; }
        public decimal LastRmdAmount { get; set; }
        public decimal LastOrdinaryIncome { get; set; }
        public decimal LastQualifiedDividend { get; set; }
        public decimal LastFedOrdinaryTax { get; set; }
        public decimal LastStateOrdinaryTax { get; set; }
        public decimal LastOrdinaryIncomeTax { get; set; }
        public decimal LastFedDividendTax { get; set; }
        public decimal LastStateDividendTax { get; set; }
        public decimal LastShortTermTax { get; set; }
        public decimal LastPenalty { get; set; }
    }

    private static Investment CloneInvestment(Investment i)
    {
        return new Investment
        {
            Type = i.Type,
            Amount = i.Amount,
            GrowthRate = i.GrowthRate,
            DividendYield = i.DividendYield,
            ShortTermGainsPercent = i.ShortTermGainsPercent,
            LongTermGainsPercent = i.LongTermGainsPercent,
            RmdAge = i.RmdAge,
            UnrealizedGains = i.UnrealizedGains,
            CostBasis = i.CostBasis
        };
    }

    private static decimal TotalAmount(IEnumerable<Investment> invs, InvestmentType type)
    {
        return invs.Where(i => i.Type == type).Sum(i => i.Amount);
    }

    private static void AddExposure(Dictionary<TaxCategory, decimal> exposures, TaxCategory category, decimal amount)
    {
        if (amount == 0m)
            return;
        if (exposures.TryGetValue(category, out var val))
            exposures[category] = val + amount;
        else
            exposures[category] = amount;
    }

    public SimulationState CreateInitialState()
    {
        var investments = p.Investments.Select(CloneInvestment).ToList();

        var state = new SimulationState
        {
            Investments = investments,
            Brokerage = investments.Where(x => x.Type == InvestmentType.Taxable).Sum(x => x.Amount),
            TotalTaxesPaid = 0m,
            RolloverTaxBucket = 0m,
            LastExternalIncome = 0m,
            LastRmdAmount = 0m,
            LastOrdinaryIncome = 0m,
            LastQualifiedDividend = 0m,
            LastFedOrdinaryTax = 0m,
            LastStateOrdinaryTax = 0m,
            LastOrdinaryIncomeTax = 0m,
            LastFedDividendTax = 0m,
            LastStateDividendTax = 0m,
            LastShortTermTax = 0m,
            LastPenalty = 0m
        };

        foreach (var kvp in baseBrackets)
            state.Brackets[kvp.Key] = kvp.Value.Select(b => new TaxBracket { Lower = b.Lower, Upper = b.Upper, Rate = b.Rate }).ToList();
        foreach (var kvp in baseRates)
            state.Rates[kvp.Key] = kvp.Value;
        return state;
    }

    private SimulationState CloneState(SimulationState s)
    {
        var state = new SimulationState
        {
            Investments = s.Investments.Select(CloneInvestment).ToList(),
            Brokerage = s.Investments.Where(x => x.Type == InvestmentType.Taxable).Sum(x => x.Amount),
            TotalTaxesPaid = s.TotalTaxesPaid,
            RolloverTaxBucket = s.RolloverTaxBucket,
            LastExternalIncome = s.LastExternalIncome,
            LastRmdAmount = s.LastRmdAmount,
            LastOrdinaryIncome = s.LastOrdinaryIncome,
            LastQualifiedDividend = s.LastQualifiedDividend,
            LastFedOrdinaryTax = s.LastFedOrdinaryTax,
            LastStateOrdinaryTax = s.LastStateOrdinaryTax,
            LastOrdinaryIncomeTax = s.LastOrdinaryIncomeTax,
            LastFedDividendTax = s.LastFedDividendTax,
            LastStateDividendTax = s.LastStateDividendTax,
            LastShortTermTax = s.LastShortTermTax,
            LastPenalty = s.LastPenalty
        };
        foreach (var kvp in s.Brackets)
            state.Brackets[kvp.Key] = kvp.Value.Select(b => new TaxBracket { Lower = b.Lower, Upper = b.Upper, Rate = b.Rate }).ToList();
        foreach (var kvp in s.Rates)
            state.Rates[kvp.Key] = kvp.Value;
        return state;
    }

    public SimulationState Step(
        SimulationState state,
        decimal rollover,
        int age,
        out decimal ordinaryTax,
        out decimal dividendTax,
        out decimal brokerageWithdrawn,
        out decimal preTaxWithdrawn,
        out decimal rothWithdrawn,
        out decimal rolloverTax)
    {
        var s = CloneState(state);
        ApplyBracketChanges(s, age);
        var exposures = new Dictionary<TaxCategory, decimal>();
        var preTaxInvs = s.Investments.Where(i => i.Type == InvestmentType.PreTax).ToList();
        var rothInvs = s.Investments.Where(i => i.Type == InvestmentType.Roth).ToList();
        var taxableInvs = s.Investments.Where(i => i.Type == InvestmentType.Taxable).ToList();

        decimal preTaxTotal = preTaxInvs.Sum(i => i.Amount);
        rollover = Math.Min(rollover, preTaxTotal);

        ApplyGrowth(s, taxableInvs, exposures, out decimal totalDividend, out decimal realizedShort, out decimal realizedLong, out decimal avgGrowthRate);
        decimal externalIncome = CalculateExternalIncome(age, exposures);
        decimal qualified = totalDividend + realizedLong;

        WithdrawSimple(preTaxInvs, rollover);
        if (rothInvs.Count == 0)
        {
            var newRoth = new Investment { Type = InvestmentType.Roth };
            s.Investments.Add(newRoth);
            rothInvs.Add(newRoth);
        }
        rothInvs[0].Amount += rollover;

        s.RolloverTaxBucket *= (1 + avgGrowthRate);

        decimal rmdAmount = ProcessRmds(age, preTaxInvs, taxableInvs);

        s.Brokerage = taxableInvs.Sum(i => i.Amount);

        decimal currentBudget = p.AnnualBudget * (decimal)Math.Pow((double)(1 + p.BracketInflation), age - p.StartAge);

        WithdrawForBudget(s, currentBudget, totalDividend, realizedShort, ref realizedLong, externalIncome, rmdAmount, taxableInvs, preTaxInvs, rothInvs,
            out brokerageWithdrawn, out preTaxWithdrawn, out rothWithdrawn);

        AddExposure(exposures, TaxCategory.FederalOrdinary, rollover + preTaxWithdrawn + rmdAmount);
        if (s.Brackets.ContainsKey(TaxCategory.StateOrdinary))
            AddExposure(exposures, TaxCategory.StateOrdinary, rollover + preTaxWithdrawn + rmdAmount);

        ComputeTaxesAndPenalties(s, age, rollover, preTaxWithdrawn, rmdAmount, taxableInvs, exposures,
            out ordinaryTax, out dividendTax, out rolloverTax);

        InflateBrackets(s);
        s.Brokerage = taxableInvs.Sum(i => i.Amount);

        return s;
    }

    private static void ApplyGrowth(
        SimulationState s,
        List<Investment> taxableInvs,
        Dictionary<TaxCategory, decimal> exposures,
        out decimal totalDividend,
        out decimal realizedShort,
        out decimal realizedLong,
        out decimal avgGrowthRate)
    {
        totalDividend = 0m;
        realizedShort = 0m;
        realizedLong = 0m;
        decimal growthWeighted = 0m;

        foreach (var inv in s.Investments)
        {
            decimal div = inv.Amount * inv.DividendYield;
            inv.Amount += div;
            if (inv.Type == InvestmentType.Taxable)
                totalDividend += div;

            if (inv.TaxPortions != null)
            {
                foreach (var tp in inv.TaxPortions)
                {
                    if (tp.Category == TaxCategory.FederalQualifiedDividend || tp.Category == TaxCategory.StateQualifiedDividend)
                        AddExposure(exposures, tp.Category, div * tp.Portion);
                }
            }

            decimal growth = inv.Amount * inv.GrowthRate;
            inv.Amount += growth;
            if (inv.Type == InvestmentType.Taxable)
            {
                decimal st = growth * inv.ShortTermGainsPercent;
                decimal lt = growth * inv.LongTermGainsPercent;
                inv.UnrealizedGains += growth - st - lt;
                realizedShort += st;
                realizedLong += lt;
                growthWeighted += inv.GrowthRate * inv.Amount;

                if (inv.TaxPortions != null)
                {
                    foreach (var tp in inv.TaxPortions)
                    {
                        if (tp.Category == TaxCategory.ShortTermCapitalGains)
                            AddExposure(exposures, tp.Category, st * tp.Portion);
                        if (tp.Category == TaxCategory.FederalQualifiedDividend || tp.Category == TaxCategory.StateQualifiedDividend)
                            AddExposure(exposures, tp.Category, lt * tp.Portion);
                    }
                }
            }
        }

        s.Brokerage = taxableInvs.Sum(i => i.Amount);
        avgGrowthRate = s.Brokerage > 0m ? growthWeighted / s.Brokerage : 0m;
    }

    private decimal CalculateExternalIncome(int age, Dictionary<TaxCategory, decimal> exposures)
    {
        decimal externalIncome = 0m;
        if (p.ExternalIncomes != null)
        {
            foreach (var inc in p.ExternalIncomes)
            {
                bool pay = inc.OneTime
                    ? age == inc.StartAge
                    : inc.EndAge.HasValue
                        ? age >= inc.StartAge && age <= inc.EndAge.Value
                        : age >= inc.StartAge;
                if (pay)
                {
                    decimal mult = (decimal)Math.Pow((double)(1 + p.BracketInflation), age - inc.StartAge);
                    decimal amt = inc.Amount * mult;
                    externalIncome += amt;
                    if (inc.TaxPortions != null)
                    {
                        foreach (var tp in inc.TaxPortions)
                            AddExposure(exposures, tp.Category, amt * tp.Portion);
                    }
                }
            }
        }
        return externalIncome;
    }

    private static void WithdrawSimple(List<Investment> invs, decimal amount)
    {
        for (int i = 0; i < invs.Count && amount > 0m; i++)
        {
            decimal take = Math.Min(amount, invs[i].Amount);
            invs[i].Amount -= take;
            amount -= take;
        }
    }

    private decimal ProcessRmds(int age, List<Investment> preTaxInvs, List<Investment> taxableInvs)
    {
        decimal total = 0m;
        foreach (var inv in preTaxInvs)
        {
            if (age >= inv.RmdAge && inv.Amount > 0m)
            {
                decimal divisor = 0m;
                if (p.RmdDivisors != null && p.RmdDivisors.TryGetValue(age, out var overrideDiv))
                    divisor = overrideDiv;
                else if (RmdTables.UniformLifetimeDivisors.TryGetValue(age, out var d))
                    divisor = d;
                else
                    divisor = RmdTables.UniformLifetimeDivisors[RmdTables.UniformLifetimeDivisors.Keys.Max()];

                if (divisor > 0m)
                {
                    decimal part = Math.Min(inv.Amount / divisor, inv.Amount);
                    inv.Amount -= part;
                    total += part;
                }
            }
        }

        if (total > 0m)
        {
            if (taxableInvs.Count > 0)
                taxableInvs[0].Amount += total;
            else
                taxableInvs.Add(new Investment { Type = InvestmentType.Taxable, Amount = total });
        }
        return total;
    }

    private static void WithdrawForBudget(
        SimulationState s,
        decimal currentBudget,
        decimal totalDividend,
        decimal realizedShort,
        ref decimal realizedLong,
        decimal externalIncome,
        decimal rmdAmount,
        List<Investment> taxableInvs,
        List<Investment> preTaxInvs,
        List<Investment> rothInvs,
        out decimal brokerageWithdrawn,
        out decimal preTaxWithdrawn,
        out decimal rothWithdrawn)
    {
        decimal remainingBudget = currentBudget;

        decimal fromDividends = Math.Min(remainingBudget, totalDividend);
        remainingBudget -= fromDividends;

        decimal realizedGains = realizedShort + realizedLong;
        decimal fromGains = Math.Min(remainingBudget, realizedGains);
        remainingBudget -= fromGains;

        decimal fromExternal = Math.Min(remainingBudget, externalIncome + rmdAmount);
        remainingBudget -= fromExternal;

        decimal excessExternal = externalIncome + rmdAmount - fromExternal;
        if (excessExternal > 0 && s.Investments.Count > 0)
            s.Investments[0].Amount += excessExternal;

        brokerageWithdrawn = Math.Min(remainingBudget, s.Brokerage);
        decimal saleGain = WithdrawFromInvestments(taxableInvs, brokerageWithdrawn);
        realizedLong += saleGain;
        remainingBudget -= brokerageWithdrawn;

        preTaxWithdrawn = Math.Min(remainingBudget, preTaxInvs.Sum(i => i.Amount));
        WithdrawSimple(preTaxInvs, preTaxWithdrawn);
        remainingBudget -= preTaxWithdrawn;

        rothWithdrawn = Math.Min(remainingBudget, rothInvs.Sum(i => i.Amount));
        WithdrawSimple(rothInvs, rothWithdrawn);
        remainingBudget -= rothWithdrawn;

        if (remainingBudget > 0)
            throw new InvalidOperationException("Insufficient funds to cover budget");

        s.Brokerage = taxableInvs.Sum(i => i.Amount);
    }

    private void ComputeTaxesAndPenalties(
        SimulationState s,
        int age,
        decimal rollover,
        decimal preTaxWithdrawn,
        decimal rmdAmount,
        List<Investment> taxableInvs,
        Dictionary<TaxCategory, decimal> exposures,
        out decimal ordinaryTax,
        out decimal dividendTax,
        out decimal rolloverTax)
    {
        decimal ordinaryIncome = exposures.GetValueOrDefault(TaxCategory.FederalOrdinary);
        decimal taxable = Math.Max(ordinaryIncome - p.StandardDeduction - p.BizLosses - p.CapitalLosses, 0m);
        decimal fedOrdTax = s.Brackets.TryGetValue(TaxCategory.FederalOrdinary, out var fedBr) ? CalculateTaxes(taxable, fedBr) : 0m;
        decimal stateOrdTax = s.Brackets.TryGetValue(TaxCategory.StateOrdinary, out var stBr) ? CalculateTaxes(taxable, stBr) : 0m;
        decimal qualified = exposures.GetValueOrDefault(TaxCategory.FederalQualifiedDividend);
        decimal fedDivTax = s.Brackets.TryGetValue(TaxCategory.FederalQualifiedDividend, out var fedDivBr) ? CalculateDividendTaxes(qualified, ordinaryIncome, fedDivBr) : 0m;
        decimal stateDivTax = s.Brackets.TryGetValue(TaxCategory.StateQualifiedDividend, out var stDivBr) ? CalculateDividendTaxes(exposures.GetValueOrDefault(TaxCategory.StateQualifiedDividend), ordinaryIncome, stDivBr) : 0m;
        decimal ficaTax = exposures.GetValueOrDefault(TaxCategory.Fica) * s.Rates.GetValueOrDefault(TaxCategory.Fica);
        decimal shortTermTax = exposures.GetValueOrDefault(TaxCategory.ShortTermCapitalGains) * s.Rates.GetValueOrDefault(TaxCategory.ShortTermCapitalGains);

        ordinaryTax = fedOrdTax + stateOrdTax + ficaTax + shortTermTax;
        dividendTax = fedDivTax + stateDivTax;
        decimal taxes = ordinaryTax + dividendTax;

        decimal baseOrdinaryIncome = preTaxWithdrawn;
        decimal baseTaxable = Math.Max(baseOrdinaryIncome - p.StandardDeduction - p.BizLosses - p.CapitalLosses, 0m);
        decimal baseOrdTax = s.Brackets.TryGetValue(TaxCategory.FederalOrdinary, out fedBr) ? CalculateTaxes(baseTaxable, fedBr) : 0m;
        decimal baseStateOrdTax = s.Brackets.TryGetValue(TaxCategory.StateOrdinary, out stBr) ? CalculateTaxes(baseTaxable, stBr) : 0m;
        decimal baseDividendTax = s.Brackets.TryGetValue(TaxCategory.FederalQualifiedDividend, out fedDivBr) ? CalculateDividendTaxes(qualified, baseOrdinaryIncome, fedDivBr) : 0m;
        decimal baseStateDividendTax = s.Brackets.TryGetValue(TaxCategory.StateQualifiedDividend, out stDivBr) ? CalculateDividendTaxes(exposures.GetValueOrDefault(TaxCategory.StateQualifiedDividend), baseOrdinaryIncome, stDivBr) : 0m;
        rolloverTax = taxes - (baseOrdTax + baseStateOrdTax + baseDividendTax + baseStateDividendTax);

        decimal penalty = age < p.EarlyWithdrawalAge ? preTaxWithdrawn * p.EarlyWithdrawalPenalty : 0m;

        s.TotalTaxesPaid += taxes + penalty;
        WithdrawFromInvestments(taxableInvs, taxes + penalty);
        s.RolloverTaxBucket += rolloverTax;

        s.LastExternalIncome = exposures.GetValueOrDefault(TaxCategory.FederalOrdinary) - (rollover + preTaxWithdrawn + rmdAmount);
        s.LastRmdAmount = rmdAmount;
        s.LastOrdinaryIncome = ordinaryIncome;
        s.LastQualifiedDividend = qualified;
        s.LastFedOrdinaryTax = fedOrdTax;
        s.LastStateOrdinaryTax = stateOrdTax;
        s.LastOrdinaryIncomeTax = ficaTax;
        s.LastFedDividendTax = fedDivTax;
        s.LastStateDividendTax = stateDivTax;
        s.LastShortTermTax = shortTermTax;
        s.LastPenalty = penalty;
    }

    private void ApplyBracketChanges(SimulationState s, int age)
    {
        foreach (var kvp in bracketChanges)
        {
            var change = kvp.Value.FirstOrDefault(c => c.StartAge == age);
            if (change != null)
                s.Brackets[kvp.Key] = change.Brackets.Select(b => new TaxBracket { Lower = b.Lower, Upper = b.Upper, Rate = b.Rate }).ToList();
        }
        foreach (var kvp in rateChanges)
        {
            var change = kvp.Value.FirstOrDefault(c => c.StartAge == age);
            if (change != null)
                s.Rates[kvp.Key] = change.Rate;
        }
    }

    private void InflateBrackets(SimulationState s)
    {
        foreach (var list in s.Brackets.Values)
        {
            foreach (var b in list)
            {
                b.Lower *= (1 + p.BracketInflation);
                b.Upper *= (1 + p.BracketInflation);
            }
        }
    }

    private static decimal WithdrawFromInvestments(List<Investment> invs, decimal amount)
    {
        decimal realized = 0m;
        invs.Sort((a, b) =>
        {
            decimal aRatio = a.Amount > 0 ? (a.CostBasis ?? a.Amount) / a.Amount : 0m;
            decimal bRatio = b.Amount > 0 ? (b.CostBasis ?? b.Amount) / b.Amount : 0m;
            return bRatio.CompareTo(aRatio);
        });

        for (int i = 0; i < invs.Count && amount > 0; i++)
        {
            if (invs[i].Amount <= 0)
                continue;
            decimal take = Math.Min(amount, invs[i].Amount);
            decimal basis = invs[i].CostBasis ?? invs[i].Amount;
            decimal basisPortion = invs[i].Amount > 0 ? basis * (take / invs[i].Amount) : 0m;
            realized += take - basisPortion;
            invs[i].Amount -= take;
            if (invs[i].CostBasis.HasValue)
                invs[i].CostBasis -= basisPortion;
            amount -= take;
        }
        return realized;
    }


    public RolloverSimulator(SimulationParams parameters)
    {
        p = parameters;
        var schedules = parameters.TaxSchedules ?? new Dictionary<TaxCategory, TaxSchedule>();
        foreach (var kvp in schedules)
        {
            if (kvp.Value.Brackets != null)
                baseBrackets[kvp.Key] = kvp.Value.Brackets.Select(b => new TaxBracket { Lower = b.Lower, Upper = b.Upper, Rate = b.Rate }).ToList();
            if (kvp.Value.FlatRate.HasValue)
                baseRates[kvp.Key] = kvp.Value.FlatRate.Value;
            bracketChanges[kvp.Key] = kvp.Value.BracketChanges ?? new List<BracketChange>();
            rateChanges[kvp.Key] = kvp.Value.RateChanges ?? new List<RateChange>();
        }
    }

    public (decimal Roth, decimal PreTax, decimal Brokerage, decimal NetWorth, decimal AfterTaxNetWorth, decimal TaxesPaid, decimal RolloverTaxBucket) Simulate(
        decimal[] annualRollovers,
        List<YearlyBreakdown>? breakdown = null)
    {
        if (annualRollovers.Length != Years)
            throw new ArgumentException($"Expected {Years} rollover entries", nameof(annualRollovers));

        var state = CreateInitialState();

        for (int i = 0; i < Years; i++)
        {
            decimal rollover = annualRollovers[i];
            decimal startingPreTax = TotalAmount(state.Investments, InvestmentType.PreTax);
            decimal startingRoth = TotalAmount(state.Investments, InvestmentType.Roth);
            decimal startingBrokerage = state.Brokerage;
            decimal startingDividendIncome =
                state.Investments.Where(inv => inv.Type == InvestmentType.Taxable)
                    .Sum(inv => inv.Amount * inv.DividendYield);
            decimal startingRolloverBucket = state.RolloverTaxBucket;
            decimal startingBudget = p.AnnualBudget *
                (decimal)Math.Pow((double)(1 + p.BracketInflation), i);


            state = Step(
                state,
                rollover,
                p.StartAge + i,
                out decimal ordinaryTax,
                out decimal dividendTax,
                out decimal brokerageWithdrawn,
                out decimal preTaxWithdrawn,
                out decimal rothWithdrawn,
                out decimal rolloverTax);

            decimal ordinaryIncome = Math.Min(rollover, startingPreTax) +
                preTaxWithdrawn + state.LastExternalIncome + state.LastRmdAmount;
            decimal taxableIncome = Math.Max(ordinaryIncome - p.StandardDeduction - p.BizLosses - p.CapitalLosses, 0m);
            decimal totalTaxes = ordinaryTax + dividendTax;
            decimal endingNetWorth =
                TotalAmount(state.Investments, InvestmentType.Roth) +
                TotalAmount(state.Investments, InvestmentType.PreTax) +
                state.Brokerage;
            decimal endingAfterTaxNetWorth = CalculateAfterTaxNetWorth(state);

            if (breakdown is not null)
            {
                breakdown.Add(new YearlyBreakdown
                {
                    Age = p.StartAge + i,
                    StartingPreTax = startingPreTax,
                    StartingRoth = startingRoth,
                    StartingBrokerage = startingBrokerage,
                    StartingRolloverTaxBucket = startingRolloverBucket,
                    DividendIncome = startingDividendIncome,
                    Rollover = Math.Min(rollover, startingPreTax),
                    OrdinaryIncome = state.LastOrdinaryIncome,
                    QualifiedDividendIncome = state.LastQualifiedDividend,
                    TaxableIncome = taxableIncome,
                    OrdinaryTax = ordinaryTax,
                    DividendTax = dividendTax,
                    TotalTaxes = totalTaxes,
                    RolloverTax = rollover == 0m ? 0m : rolloverTax,
                    Budget = startingBudget,
                    BrokerageWithdrawn = brokerageWithdrawn,
                    PreTaxWithdrawn = preTaxWithdrawn,
                    RothWithdrawn = rothWithdrawn,
                    EndingPreTax = TotalAmount(state.Investments, InvestmentType.PreTax),
                    EndingRoth = TotalAmount(state.Investments, InvestmentType.Roth),
                    EndingBrokerage = state.Brokerage,
                    EndingRolloverTaxBucket = state.RolloverTaxBucket,
                    ExternalIncome = state.LastExternalIncome,
                    RmdAmount = state.LastRmdAmount,
                    FedOrdinaryTax = state.LastFedOrdinaryTax,
                    StateOrdinaryTax = state.LastStateOrdinaryTax,
                    OrdinaryIncomeTax = state.LastOrdinaryIncomeTax,
                    FedDividendTax = state.LastFedDividendTax,
                    StateDividendTax = state.LastStateDividendTax,
                    ShortTermTax = state.LastShortTermTax,
                    Penalty = state.LastPenalty,
                    EndingNetWorth = endingNetWorth,
                    EndingAfterTaxNetWorth = endingAfterTaxNetWorth
                });
            }
        }

        decimal netWorth =
            TotalAmount(state.Investments, InvestmentType.Roth) +
            TotalAmount(state.Investments, InvestmentType.PreTax) +
            state.Brokerage;
        decimal afterTaxNetWorth = CalculateAfterTaxNetWorth(state);
        return (
            TotalAmount(state.Investments, InvestmentType.Roth),
            TotalAmount(state.Investments, InvestmentType.PreTax),
            state.Brokerage,
            netWorth,
            afterTaxNetWorth,
            state.TotalTaxesPaid,
            state.RolloverTaxBucket);
    }

    private decimal CalculateAfterTaxNetWorth(SimulationState state)
    {
        decimal preTaxBal = TotalAmount(state.Investments, InvestmentType.PreTax);
        decimal rothBal = TotalAmount(state.Investments, InvestmentType.Roth);
        decimal taxable = Math.Max(preTaxBal - p.StandardDeduction - p.BizLosses - p.CapitalLosses, 0m);
        decimal taxOnRemaining = state.Brackets.TryGetValue(TaxCategory.FederalOrdinary, out var fedBr) ? CalculateTaxes(taxable, fedBr) : 0m;
        decimal penalty = preTaxBal * p.PreTaxPenaltyRate;
        return rothBal + state.Brokerage + preTaxBal - taxOnRemaining - penalty;
    }


    private decimal CalculateTaxes(decimal taxable, List<TaxBracket> brackets)
    {
        decimal taxes = 0;
        decimal remaining = taxable;

        foreach (var bracket in brackets)
        {
            if (remaining <= 0)
                break;
            decimal amountInBracket = Math.Min(remaining, bracket.Upper - bracket.Lower);
            if (amountInBracket > 0)
                taxes += amountInBracket * bracket.Rate;
            remaining -= amountInBracket;
        }
        return taxes;
    }

    private decimal CalculateDividendTaxes(decimal qualifiedDividend, decimal ordinaryIncome, List<TaxBracket> brackets)
    {
        decimal taxes = 0m;
        decimal remaining = qualifiedDividend;
        foreach (var bracket in brackets)
        {
            if (remaining <= 0)
                break;

            decimal bracketLower = bracket.Lower;
            decimal bracketUpper = bracket.Upper;

            decimal available = Math.Max(0, bracketUpper - Math.Max(bracketLower, ordinaryIncome));

            decimal amountInBracket = Math.Min(remaining, available);
            if (amountInBracket > 0)
                taxes += amountInBracket * bracket.Rate;
            remaining -= amountInBracket;
        }
        if (remaining > 0)
            taxes += remaining * brackets.Last().Rate;
        return taxes;
    }
}
