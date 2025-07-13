using System.ComponentModel.DataAnnotations;

namespace RetirementPlanner.Models;

public class Person
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateOnly BirthDate { get; set; }
}
