using System.Collections.Generic;
namespace Money.Domain;
public class SimulationParams
{
    // All investment accounts including pre-tax, Roth and taxable
    public List<Investment> Investments { get; set; } = new();
    public int TargetAge { get; set; }
    public int StartAge { get; set; }
    public decimal StandardDeduction { get; set; }
    public decimal BizLosses { get; set; }
    public decimal CapitalLosses { get; set; }
    public decimal BracketInflation { get; set; }      // e.g. 0.0363m
    public decimal AnnualBudget { get; set; } = 0m;

    // Optional override for IRS life expectancy divisors by age
    public Dictionary<int, decimal>? RmdDivisors { get; set; }

    // Penalty for withdrawing pretax funds before a certain age
    public int EarlyWithdrawalAge { get; set; } = 59;
    public decimal EarlyWithdrawalPenalty { get; set; } = 0.1m;

    // Additional income sources (Social Security, inheritances, etc.)
    public List<IncomeItem>? ExternalIncomes { get; set; }

    public Dictionary<TaxCategory, TaxSchedule> TaxSchedules { get; set; } = new();

    // Penalty applied to the remaining pre-tax balance when
    // calculating after-tax net worth
    public decimal PreTaxPenaltyRate { get; set; } = 0m;
}

