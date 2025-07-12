using Money.Domain;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ClosedXML.Excel;

namespace Money.Optimization;

public static class ReportUtils
{
    public static void PrintBreakdown(RolloverSimulator simulator, decimal[] schedule)
    {
        var breakdown = new List<YearlyBreakdown>();
        var result = simulator.Simulate(schedule, breakdown);

        Console.WriteLine("Age\tStartNet\tRollover\tTaxes\tBudget\tNetGain\tEndNet");
        foreach (var y in breakdown)
        {
            decimal startNet = y.StartingPreTax + y.StartingRoth + y.StartingBrokerage;
            decimal netGain = y.EndingNetWorth - startNet;
            Console.WriteLine($"{y.Age}\t{startNet:C}\t{y.Rollover:C}\t{y.TotalTaxes:C}\t{y.Budget:C}\t{netGain:C}\t{y.EndingNetWorth:C}");
        }
        Console.WriteLine($"After-Tax Net Worth:\t{result.AfterTaxNetWorth:C}");
    }


    public static void AddBreakdownWorksheet(RolloverSimulator simulator, decimal[] schedule, IXLWorkbook workbook, string sheetName)
    {
        var breakdown = new List<YearlyBreakdown>();
        simulator.Simulate(schedule, breakdown);

        var ws = workbook.AddWorksheet(sheetName);

        // Column order with Age first and no ending balances
        string[] headers = new[]
        {
            "Age","NetWorth","Budget","StartingPreTax","StartingRoth","StartingBrokerage","StartingRolloverTaxBucket",
            "DividendIncome","OrdinaryIncome","QualifiedDividendIncome","TaxableIncome","Rollover",
            "ExternalIncome","RmdAmount",
            "OrdinaryTax","DividendTax","ShortTermTax","FedOrdinaryTax","StateOrdinaryTax","OrdinaryIncomeTax",
            "FedDividendTax","StateDividendTax","TotalTaxes",
            "RolloverTax","Penalty",
            "BrokerageWithdrawn","PreTaxWithdrawn","RothWithdrawn",
            "EndingPreTax","EndingRoth","EndingBrokerage","EndingRolloverTaxBucket","EndingNetWorth","EndingAfterTaxNetWorth"
        };

        // Group ranges for merged headers
        var groups = new (string Name, int Start, int End)[]
        {
            ("Balances", 2, 7),
            ("Income / Growth", 8, 14),
            ("Taxes", 15, 25),
            ("Other", 26, 34)
        };

        // Draw the group header row
        foreach (var (name, start, end) in groups)
        {
            var rng = ws.Range(1, start, 1, end);
            rng.Merge();
            rng.Value = name;
            rng.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rng.Style.Fill.BackgroundColor = name switch
            {
                "Balances" => XLColor.LightBlue,
                "Income / Growth" => XLColor.LightGreen,
                "Taxes" => XLColor.LightYellow,
                _ => XLColor.Plum
            };
        }

        // Column header row with wrapping and spaces
        for (int c = 0; c < headers.Length; c++)
        {
            string spaced = System.Text.RegularExpressions.Regex.Replace(headers[c], "(?<!^)([A-Z])", " $1");
            ws.Cell(2, c + 1).Value = spaced;
            ws.Cell(2, c + 1).Style.Alignment.WrapText = true;
        }

        int row = 3;
        foreach (var y in breakdown)
        {
            object[] values = new object[]
            {
                y.Age,
                0m, // placeholder for Net Worth formula
                y.Budget, y.StartingPreTax, y.StartingRoth, y.StartingBrokerage, y.StartingRolloverTaxBucket,
                y.DividendIncome, y.OrdinaryIncome, y.QualifiedDividendIncome, y.TaxableIncome, y.Rollover,
                y.ExternalIncome, y.RmdAmount,
                y.OrdinaryTax, y.DividendTax, y.ShortTermTax, y.FedOrdinaryTax, y.StateOrdinaryTax,
                y.OrdinaryIncomeTax, y.FedDividendTax, y.StateDividendTax, y.TotalTaxes,
                y.RolloverTax, y.Penalty,
                y.BrokerageWithdrawn, y.PreTaxWithdrawn, y.RothWithdrawn,
                y.EndingPreTax, y.EndingRoth, y.EndingBrokerage, y.EndingRolloverTaxBucket,
                0m, // placeholder for Ending Net Worth formula
                y.EndingAfterTaxNetWorth
            };
            for (int c = 0; c < values.Length; c++)
            {
                if (c == 1)
                    continue; // formula filled later

                var v = values[c];
                if (v is decimal d)
                    ws.Cell(row, c + 1).Value = (double)d;
                else if (v is int i)
                    ws.Cell(row, c + 1).Value = i;
                else if (v is double db)
                    ws.Cell(row, c + 1).Value = db;
                else if (v is float f)
                    ws.Cell(row, c + 1).Value = (double)f;
                else if (v is string s)
                    ws.Cell(row, c + 1).Value = s;
                else
                    ws.Cell(row, c + 1).Value = v?.ToString() ?? string.Empty;
            }


            string sp = ColumnLetter(4);  // StartingPreTax column
            string sr = ColumnLetter(5);  // StartingRoth column
            string sb = ColumnLetter(6);  // StartingBrokerage column
            string srt = ColumnLetter(7); // StartingRolloverTaxBucket column
            string ep = ColumnLetter(29); // EndingPreTax column
            string er = ColumnLetter(30); // EndingRoth column
            string eb = ColumnLetter(31); // EndingBrokerage column
            string ert = ColumnLetter(32); // EndingRolloverTaxBucket column

            ws.Cell(row, 2).FormulaA1 = $"={sp}{row}+{sr}{row}+{sb}{row}";

            if (row > 3)
            {
                ws.Cell(row, 4).FormulaA1 = $"={ep}{row-1}";
                ws.Cell(row, 5).FormulaA1 = $"={er}{row-1}";
                ws.Cell(row, 6).FormulaA1 = $"={eb}{row-1}";
                ws.Cell(row, 7).FormulaA1 = $"={ert}{row-1}";
            }

            ws.Cell(row, 23).FormulaA1 = $"={ColumnLetter(15)}{row}+{ColumnLetter(16)}{row}";
            ws.Cell(row, 33).FormulaA1 = $"={ep}{row}+{er}{row}+{eb}{row}";

            row++;
        }

        // Apply accounting format with no decimals to money columns
        const string moneyFormat = "$#,##0";
        for (int c = 1; c <= headers.Length; c++)
        {
            if (c == 1) // Age column
                continue;
            ws.Column(c).Style.NumberFormat.Format = moneyFormat;
        }

        ws.Columns().AdjustToContents();
    }

    private static string ColumnLetter(int column)
    {
        string s = string.Empty;
        while (column > 0)
        {
            column--;
            s = (char)('A' + (column % 26)) + s;
            column /= 26;
        }
        return s;
    }

}
