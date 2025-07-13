using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetirementPlanner.Models;

public class InvestmentDistributionConfig
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DistributionCategory Category { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AnnualPercentage { get; set; }

    [ForeignKey("Investment")]
    public int InvestmentId { get; set; }
    public Investment Investment { get; set; } = null!;
}
