using System.Collections.Generic;

namespace Money.Domain;

public enum InvestmentType
{
    PreTax,
    Roth,
    Taxable
}

public class Investment
{
    public InvestmentType Type { get; set; } = InvestmentType.Taxable;
    public decimal Amount { get; set; }
    public decimal GrowthRate { get; set; }
    public decimal DividendYield { get; set; }
    public decimal ShortTermGainsPercent { get; set; }
    public decimal LongTermGainsPercent { get; set; }
    // Age when RMDs begin for this investment
    public int RmdAge { get; set; } = 73;
    public decimal UnrealizedGains { get; set; } = 0m;
    public decimal? CostBasis { get; set; }
    public List<TaxPortion>? TaxPortions { get; set; }
}
