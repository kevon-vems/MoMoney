using System.Collections.Generic;
namespace Money.Domain;

public class SimulationConfig
{
    public string ScenarioName { get; set; } = "Default";
    public SimulationParams SimulationParams { get; set; } = new();
    public Dictionary<TaxCategory, TaxSchedule> TaxSchedules { get; set; } = new();
}
