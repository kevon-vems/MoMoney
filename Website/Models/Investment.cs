using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetirementPlanner.Models;

public class Investment
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Scenario")]
    public int ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public InvestmentType InvestmentType { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentBalance { get; set; }

    public virtual ICollection<InvestmentDistributionConfig> DistributionConfig { get; set; } = new List<InvestmentDistributionConfig>();

    [Column(TypeName = "decimal(18,2)")]
    public decimal ExpectedDividendYield { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ExpectedGrowthRate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ExpectedReturnOfCapitalRate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ExpenseRatio { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ExpectedTurnoverRate { get; set; }

    public bool IsFederalTaxExempt { get; set; }

    public bool IsStateTaxExempt { get; set; }

    public int WithdrawalPriority { get; set; }

    public bool RequiresRMD { get; set; }

    public int? RMDStartAge { get; set; }

    [ForeignKey("Person")]
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
}
