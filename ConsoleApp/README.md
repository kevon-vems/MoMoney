# Money

Money is a console application that models multi-year Roth conversions. It reads `config.json`, runs a simulation and writes an Excel workbook with the results.

The program is intentionally simple: each year a `RolloverSimulator` applies growth, dividends, required minimum distributions, taxes and withdrawals. A built-in strategy converts just enough pre‑tax funds to stay under the 24% federal bracket.

## Building and running

Use the `build.sh` script. It uses the local .NET SDK if available or falls back to Docker or a temporary SDK download.

```bash
./build.sh
```

After building you can run the program with:

```bash
.dotnet/dotnet run --no-build
```

All source files are under the `src` folder with unit tests in `tests`.

This reads `config.json`, executes the simulation and writes an Excel workbook in the project directory.

## Configuration

Important fields in `config.json`:

| Field | Meaning |
|-------|---------|
| `Investments` | List of accounts with `Type` (`PreTax`, `Roth`, `Taxable`), starting `Amount`, `GrowthRate`, dividend information, optional `CostBasis` and the age when RMDs start. |
| `ScenarioName` | Label used when naming output files. |
| `StartAge` / `TargetAge` | Ages at the beginning and end of the simulation. |
| `StandardDeduction`, `BizLosses`, `CapitalLosses` | Values that reduce taxable income. |
| `BracketInflation` | Annual inflation applied to tax brackets and the spending budget. |
| `AnnualBudget` | Amount spent every year. |
| `RmdDivisors` | Optional overrides for IRS RMD divisors by age. |
| `EarlyWithdrawalAge` / `EarlyWithdrawalPenalty` | Penalty for taking pre‑tax funds early. |
| `ExternalIncomes` | Additional income sources with a start age, optional end age and one‑time flag. |
| `TaxSchedules` | Dictionary of `TaxCategory` values to either a bracket list or a flat rate. Rates and brackets can be modified at specific ages through `RateChanges` or `BracketChanges`. |
| `PreTaxPenaltyRate` | Fraction of remaining pre‑tax funds subtracted when computing after‑tax net worth. |

### Investment model

Every account is represented by an `Investment` object. During each year the simulator applies dividends and growth to all investments, handles withdrawals and RMDs, then computes federal and state taxes. Tax‑advantaged accounts simply have zero dividend and capital gain percentages. This unified approach keeps the bookkeeping consistent for brokerage and retirement balances.
Income sources and investments can optionally include `TaxPortions` entries specifying how their amounts are split across one or more `TaxCategory` values.  Each `TaxPortion` entry has a `Category` and a `Portion` that is multiplied by the original amount when tallying tax exposure.  The portions are not required to sum to one because the same income can be taxed under multiple categories (for example both `FederalOrdinary` and `Fica`).

### Taxes

Tax categories include `FederalOrdinary`, `FederalQualifiedDividend`, `StateOrdinary`,
`StateQualifiedDividend`, `ShortTermCapitalGains` and `Fica`. These names match the
values in [`TaxCategory.cs`](Domain/TaxCategory.cs) and determine which tax schedule
is used for a piece of income. In general:

* `FederalOrdinary` and `StateOrdinary` cover wages, pre‑tax withdrawals and Roth
  conversions.
* `FederalQualifiedDividend` and `StateQualifiedDividend` apply to qualified
  dividends and long‑term capital gains.
* `ShortTermCapitalGains` is a flat‑rate category used for non‑qualified gains.
* `Fica` models payroll taxes for earned income.

Each category maps to a `TaxSchedule` describing either progressive brackets or a
flat rate. Brackets are automatically increased each year using `BracketInflation`
and can optionally change at specific ages via `BracketChanges` or `RateChanges`.

Example snippet from `config.json`:

```json
"TaxSchedules": {
  "FederalOrdinary": {
    "Brackets": [
      {"Lower": 0, "Upper": 23850, "Rate": 0.10},
      {"Lower": 23850, "Upper": 96950, "Rate": 0.12}
    ],
    "BracketChanges": [
      {
        "StartAge": 58,
        "Brackets": [
          {"Lower": 0, "Upper": 23850, "Rate": 0.10},
          {"Lower": 23850, "Upper": 96950, "Rate": 0.25}
        ]
      }
    ]
  },
  "ShortTermCapitalGains": { "FlatRate": 0.05 },
  "Fica": { "FlatRate": 0.0765 }
}
```

Investments and external incomes declare the portion of their amounts that fall
into each category using `TaxPortions`:

```json
{
  "Amount": 6000,
  "StartAge": 55,
  "TaxPortions": [
    {"Category": "FederalOrdinary", "Portion": 0.1}
  ]
}
```

This indicates that ten percent of that income is taxed as federal ordinary
income. Similar entries can be added to investments so that dividends and
capital gains flow into the correct tax categories. When a single income source
is subject to multiple taxes, list multiple entries with their respective
portions. For example wages taxed for both income and payroll purposes would be
defined as:

```json
{
  "Amount": 80000,
  "StartAge": 60,
  "TaxPortions": [
    {"Category": "FederalOrdinary", "Portion": 1.0},
    {"Category": "StateOrdinary", "Portion": 1.0},
    {"Category": "Fica", "Portion": 1.0}
  ]
}
```

Each category receives the full amount, producing federal/state ordinary income
and FICA exposure simultaneously.

## Output

The program produces one Excel workbook. The file name begins with `ScenarioName` and a timestamp, e.g. `BaseScenario-final_distribution-<ticks>.xlsx`. The workbook contains a worksheet showing each year of the plan with balances, taxes, withdrawals and ending net worth.

### Worksheet columns

Each row represents one simulation year. The columns written by
`ReportUtils.AddBreakdownWorksheet` are:

| Column | Description |
|--------|-------------|
| `Age` | Age at the start of the year. |
| `NetWorth` | Sum of `StartingPreTax`, `StartingRoth` and `StartingBrokerage`. |
| `Budget` | Spending budget for the year after inflation. |
| `StartingPreTax` | Value of all pre-tax accounts at the start of the year. |
| `StartingRoth` | Value of all Roth accounts at the start of the year. |
| `StartingBrokerage` | Taxable brokerage balance at the start of the year. |
| `StartingRolloverTaxBucket` | Accumulated taxes from prior rollovers grown with investment returns (represents lost growth). |
| `DividendIncome` | Dividends earned from the taxable brokerage. |
| `OrdinaryIncome` | Total ordinary income before deductions (rollovers, withdrawals, external income and RMDs). |
| `QualifiedDividendIncome` | Dividends subject to qualified rates. |
| `TaxableIncome` | Ordinary income minus deductions. |
| `Rollover` | Amount converted from pre‑tax to Roth in this year. |
| `ExternalIncome` | Income items defined in `config.json`. |
| `RmdAmount` | Required minimum distributions for the year. |
| `OrdinaryTax` | Combined federal, state and payroll taxes on ordinary income. |
| `DividendTax` | Taxes due on qualified dividends. |
| `ShortTermTax` | Taxes on short-term capital gains. |
| `FedOrdinaryTax` | Federal portion of ordinary tax. |
| `StateOrdinaryTax` | State portion of ordinary tax. |
| `OrdinaryIncomeTax` | Payroll (FICA) taxes on earned income. |
| `FedDividendTax` | Federal tax on qualified dividends. |
| `StateDividendTax` | State tax on qualified dividends. |
| `TotalTaxes` | Sum of `OrdinaryTax` and `DividendTax`. |
| `RolloverTax` | Incremental tax caused by the current year's conversion. |
| `Penalty` | Early-withdrawal penalty if pre-tax funds are used before `EarlyWithdrawalAge`. |
| `BrokerageWithdrawn` | Amount taken from the brokerage to cover spending and taxes. |
| `PreTaxWithdrawn` | Withdrawals from pre-tax accounts for spending or RMDs. |
| `RothWithdrawn` | Withdrawals from Roth accounts. |
| `EndingPreTax` | Pre-tax account balance at year end. |
| `EndingRoth` | Roth account balance at year end. |
| `EndingBrokerage` | Brokerage balance at year end. |
| `EndingRolloverTaxBucket` | Updated rollover tax bucket after growth and the year's rollover taxes. |
| `EndingNetWorth` | Sum of `EndingPreTax`, `EndingRoth` and `EndingBrokerage`. |
| `EndingAfterTaxNetWorth` | Estimated net worth after paying tax on remaining pre-tax funds and penalties. |

## How it works

1. `Program.cs` loads `config.json` and constructs a `RolloverSimulator`.
2. `TaxBracketRolloverStrategy` computes how much to convert each year to stay under the 24% bracket.
3. `RolloverSimulator.Simulate` iterates from `StartAge` to `TargetAge`, applying growth, income, withdrawals and taxes.
4. `ReportUtils.AddBreakdownWorksheet` writes the yearly ledger to the workbook.

Adjust the parameters in `config.json` and rerun the program to explore different rollover schedules.
