using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetirementPlanner.Models;

public class InvestmentRollover
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("SourceInvestment")]
    public int SourceInvestmentId { get; set; }
    public Investment SourceInvestment { get; set; } = null!;

    [ForeignKey("DestinationInvestment")]
    public int DestinationInvestmentId { get; set; }
    public Investment DestinationInvestment { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public int Year { get; set; }

    public RolloverType RolloverType { get; set; }
}
