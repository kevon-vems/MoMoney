namespace Money.Domain;

public class YearlyBreakdown
{
    public int Age { get; init; }
    public decimal StartingPreTax { get; init; }
    public decimal StartingRoth { get; init; }
    public decimal StartingBrokerage { get; init; }
    public decimal StartingRolloverTaxBucket { get; init; }
    // Dividend amount earned from the brokerage for the year
    public decimal DividendIncome { get; init; }
    public decimal Rollover { get; init; }
    public decimal OrdinaryIncome { get; init; }
    public decimal QualifiedDividendIncome { get; init; }
    public decimal TaxableIncome { get; init; }
    public decimal OrdinaryTax { get; init; }
    public decimal DividendTax { get; init; }
    public decimal TotalTaxes { get; init; }
    public decimal RolloverTax { get; init; }
    public decimal Budget { get; init; }
    public decimal BrokerageWithdrawn { get; init; }
    public decimal PreTaxWithdrawn { get; init; }
    public decimal RothWithdrawn { get; init; }
    public decimal EndingPreTax { get; init; }
    public decimal EndingRoth { get; init; }
    public decimal EndingBrokerage { get; init; }
    public decimal EndingRolloverTaxBucket { get; init; }
    public decimal ExternalIncome { get; init; }
    public decimal RmdAmount { get; init; }
    public decimal FedOrdinaryTax { get; init; }
    public decimal StateOrdinaryTax { get; init; }
    public decimal OrdinaryIncomeTax { get; init; }
    public decimal FedDividendTax { get; init; }
    public decimal StateDividendTax { get; init; }
    public decimal ShortTermTax { get; init; }
    public decimal Penalty { get; init; }
    public decimal EndingNetWorth { get; init; }
    public decimal EndingAfterTaxNetWorth { get; init; }
}
