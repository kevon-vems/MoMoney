using Money.Domain;
using Money.Optimization;
using Money;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using System;
using System.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length > 0 && args[0].Equals("combine", StringComparison.OrdinalIgnoreCase))
        {
            string output = args.Length > 1 ? args[1] : "combined-final_distribution.xlsx";
            CsvCombiner.CombineFinalDistributions(output);
            return;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());

        var config = JsonSerializer.Deserialize<SimulationConfig>(File.ReadAllText("config.json"), options)!;
        if (config.SimulationParams.Investments == null || config.SimulationParams.Investments.Count == 0)
        {
            Console.Error.WriteLine("No investments configured in config.json");
            return;
        }
        var simParams = config.SimulationParams;
        simParams.TaxSchedules = config.TaxSchedules;
        var scenarioName = config.ScenarioName;

        var workbook = new XLWorkbook();

        var simulator = new RolloverSimulator(simParams);

        decimal[] rollovers = TaxBracketRolloverStrategy.Calculate(simulator, simParams.TaxSchedules);
        var breakdown = new List<YearlyBreakdown>();
        var result = simulator.Simulate(rollovers, breakdown);

        ReportUtils.AddBreakdownWorksheet(simulator, rollovers, workbook, "24PercentStrategy");
        string xlPath = $"{scenarioName}-final_distribution-{DateTime.Now.Ticks}.xlsx";
        workbook.SaveAs(xlPath);

        Console.WriteLine($"Workbook written to {xlPath}");
        Console.WriteLine($"Final Roth at age {simParams.TargetAge}:\t{result.Roth:C}");
        Console.WriteLine($"Final Pre-Tax at age {simParams.TargetAge}:\t{result.PreTax:C}");
        Console.WriteLine($"Final Brokerage at age {simParams.TargetAge}:\t{result.Brokerage:C}");
        Console.WriteLine($"Total Taxes Paid:\t{result.TaxesPaid:C}");
        Console.WriteLine($"Total Net Worth:\t{result.NetWorth:C}");
        Console.WriteLine($"Lost Rollover Growth:\t{result.RolloverTaxBucket:C}");
    }
}
