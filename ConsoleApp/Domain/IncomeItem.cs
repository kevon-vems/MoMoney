using System.Collections.Generic;

namespace Money.Domain;

public class IncomeItem
{
    public decimal Amount { get; set; }
    public int StartAge { get; set; }
    public int? EndAge { get; set; }
    public bool OneTime { get; set; } = false;
    public List<TaxPortion>? TaxPortions { get; set; }
}
