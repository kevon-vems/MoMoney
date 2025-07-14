using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetirementPlanner.Models;

public class TaxBracket
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Scenario")]
    public int ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    [Required]
    public TaxType TaxType { get; set; }

    [Required]
    public FilingStatus FilingStatus { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinIncome { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MaxIncome { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Rate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Threshold { get; set; }

    [StringLength(100)]
    public string? State { get; set; }
}
