using ClosedXML.Excel;
using System.Globalization;

namespace Money;

public static class CsvCombiner
{
    public static void CombineFinalDistributions(string outputPath, string searchDirectory = ".")
    {
        var csvFiles = Directory.GetFiles(searchDirectory, "*final_distribution*.csv");
        if (csvFiles.Length == 0)
        {
            Console.WriteLine("No final distribution CSV files found.");
            return;
        }

        using var workbook = new XLWorkbook();

        foreach (var file in csvFiles)
        {
            var lines = File.ReadAllLines(file);
            if (lines.Length == 0)
                continue;

            string sheetName = Path.GetFileNameWithoutExtension(file);
            if (sheetName.Length > 31)
                sheetName = sheetName[^31..];

            var ws = workbook.AddWorksheet(sheetName);
            var headers = lines[0].Split(',');
            for (int c = 0; c < headers.Length; c++)
                ws.Cell(1, c + 1).Value = headers[c];

            for (int r = 1; r < lines.Length; r++)
            {
                var values = lines[r].Split(',');
                for (int c = 0; c < values.Length; c++)
                {
                    string val = values[c];
                    if (decimal.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
                        ws.Cell(r + 1, c + 1).Value = (double)d;
                    else if (int.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out var i))
                        ws.Cell(r + 1, c + 1).Value = i;
                    else
                        ws.Cell(r + 1, c + 1).Value = val;
                }
            }
        }

        workbook.SaveAs(outputPath);
        Console.WriteLine($"Workbook written to {outputPath}");
    }
}
