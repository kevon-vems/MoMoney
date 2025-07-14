using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetirementPlanner.Models;

public class Expense
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Scenario")]
    public int ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    [Required, StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal AnnualAmount { get; set; }

    public bool IsHealthcare { get; set; }

    public int StartYear { get; set; }

    public int? EndYear { get; set; }

    [StringLength(100)]
    public string Notes { get; set; } = string.Empty;
}
