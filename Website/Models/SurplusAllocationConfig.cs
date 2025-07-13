using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetirementPlanner.Models;

public class SurplusAllocationConfig
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Investment")]
    public int InvestmentId { get; set; }
    public Investment Investment { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal AllocationPercentage { get; set; }
}
