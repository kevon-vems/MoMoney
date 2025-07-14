using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetirementPlanner.Models;

public class Income
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Scenario")]
    public int ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    [Required, StringLength(100)]
    public string Source { get; set; } = string.Empty;

    [Required]
    public IncomeType IncomeType { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AnnualAmount { get; set; }

    public bool IsTaxExempt { get; set; }

    public int StartYear { get; set; }

    public int? EndYear { get; set; }

    [ForeignKey("Person")]
    public int? PersonId { get; set; }
    public Person? Person { get; set; }
}
