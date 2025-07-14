using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetirementPlanner.Models;

public class Asset
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Scenario")]
    public int ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    public bool IsLiquid { get; set; }

    public bool IsPrimaryResidence { get; set; }

    public bool IsDepreciable { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? AppreciationRate { get; set; }
}
