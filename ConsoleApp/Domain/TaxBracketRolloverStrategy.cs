using System;
using System.Collections.Generic;
using System.Linq;
using Money.Optimization;

namespace Money.Domain;

public static class TaxBracketRolloverStrategy
{
public static decimal[] Calculate(RolloverSimulator simulator, Dictionary<TaxCategory, TaxSchedule> schedules)
    {
        int years = simulator.Years;
        decimal[] schedule = new decimal[years];
        var zero = new decimal[years];
        var baseline = new List<YearlyBreakdown>();
        simulator.Simulate(zero, baseline);

        decimal inflation = simulator.Params.BracketInflation;

        var baseBrackets = schedules[TaxCategory.FederalOrdinary].Brackets ?? new List<TaxBracket>();
        var bracket24 = baseBrackets.FirstOrDefault(b => Math.Abs(b.Rate - 0.24m) < 0.001m);
        if (bracket24 == null)
            return schedule;

        for (int i = 0; i < years; i++)
        {
            decimal inflFactor = (decimal)Math.Pow((double)(1 + inflation), i);
            decimal top = bracket24.Upper * inflFactor;
            decimal taxable = baseline[i].TaxableIncome;
            decimal room = Math.Max(top - taxable, 0m);
            schedule[i] = room;
        }

        return schedule;
    }
}
