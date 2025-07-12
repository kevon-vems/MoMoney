# Root-level instructions for Codex

- All projects must target .NET 9.0 (`net9.0`). Never downgrade frameworks.
- When introducing new functionality, always add the appropriate NuGet packages to the project file.
- After any code changes, run:
    - `dotnet restore <solution>` to ensure packages are installed
    - `dotnet build <solution> --configuration Release --no-restore`

You should use the Money project as a ROUGH guide for completing this Blazor website.
Only follow it conceptually.
The high level concepts we will model first are:
[]: # Do some research on best practices for retirement applications.  Reference: https://github.com/DougPomerenke/YARP-UI as an example
[]: # Create the models for the investments, income sources, tax categories, and tax schedules based on the Money project AND the code below AND your research into best practices.
[]: # Use Entity Framework Core for data access.
[]: # Create a Blazor Server application with the following structure:
[]: # - Data: Contains the Entity Framework Core DbContext and models.
[]: # - Pages: Contains the Blazor pages for the application.
[]: # - Services: Contains any services needed for data access or business logic.
[]: # - wwwroot: Contains static files like CSS, JavaScript, and images.
[]: # - Shared: Contains shared components like navigation menus and footers.
[]: # - Layouts: Contains layout components for the application.
[]: # - Components: Contains reusable Blazor components.
[]: # - Properties: Contains the project properties and settings.
[]: # - Program.cs: The entry point for the Blazor Server application.
[]: # - Startup.cs: The startup configuration for the Blazor Server application.
[]: # - appsettings.json: The configuration file for the Blazor Server application.
[]: # - appsettings.Development.json: The development configuration file for the Blazor Server application.
[]: # - wwwroot/css/app.css: The main CSS file for the Blazor Server application.
[]: # - wwwroot/js/app.js: The main JavaScript file for the Blazor Server application.
[]: # - Configure the application to use Entity Framework with SQL Server based on the sample connection string:     "DefaultConnection": "Server=kh-server;Database=MoMoney;User Id=SA;Password={TO BE SET LATER};TrustServerCertificate=True;MultipleActiveResultSets=true;",
[]: # - Use EF Migrations
[]: # - Use Tailwind 4 for styling the application.
[]: # - Use Blazor components for the UI, including forms, tables, and charts.
[]: # - Use a responsive design to ensure the application works well on both desktop and mobile devices.
[]: # - Use Blazor's built-in authentication and authorization features to secure the application.
[]: # - Use Blazor's built-in localization features to support multiple languages.
[]: # - Use Blazor's built-in error handling features to handle exceptions and display error messages to the user.
[]: # - Use Blazor's built-in logging features to log application events and errors.
[]: # Create a basic UI layout with a navigation menu.
[]: # Create a Blazor page to display and edit the retirement planning parameters (filing status, state, simulation years, inflation rates).
[]: # Create a Blazor page to display and edit the people involved in the retirement plan.
[]: # Use Entity Framework and migrations to create the database schema.
[]: # Create a Blazor page to display and edit the investments.
[]: # Create a Blazor page to display and edit the income sources.
[]: # Create a Blazor page to display the investments and income sources.
[]: # Create a Blazor page to display and edit the tax categories and tax schedules.
[]: # Create a Blazor page to run the simulation and display the results. Install any appropriate NuGet packages to create a nice graphical display of the results.  A Line Chart, stacked bar chart, combination, or a variety of visualization methods, selectable by the user would be a good start. This will allow us to see different things and then choose the one which works best.
[]: # Create a Blazor page to download the simulation results as an Excel file.

Consider both the Money project and the concepts below when creating a data structure.  Consider simplicity and ease of use, while allowing for flexibility in the future.  The goal is to create a data structure that can be easily extended to accommodate new features and requirements as they arise.

public enum FilingStatus
{
    Single,
    MarriedFilingJointly,
    MarriedFilingSeparately,
    HeadOfHousehold,
    QualifyingWidow
}

public enum InvestmentType
{
    TaxableBrokerage,
    TraditionalIRA,
    RothIRA,
    Employer401k,
    Roth401k,
    HSA,
    Annuity,
    Pension
}

public enum IncomeType
{
    W2,
    SelfEmployment,
    TaxExempt,
    Pension,
    SocialSecurity,
    OtherTaxable
}

public enum DistributionCategory
{
    Interest,
    QualifiedDividend,
    NonQualifiedDividend,
    ShortTermCapitalGain,
    LongTermCapitalGain,
    ReturnOfCapital,
    Other
}

public enum TaxType
{
    OrdinaryIncome,
    QualifiedDividend,
    ShortTermCapitalGain,
    LongTermCapitalGain,
    NetInvestmentIncomeTax,
    AdditionalMedicareTax,
    StateOrdinaryIncome,
    StateCapitalGains
}

public enum RolloverType
{
    RothConversion,
    IraToIra,
    EmployerToIra,
    Other
}

public class RetirementPlannerContext
{
    public FilingStatus FilingStatus { get; set; }
    public string State { get; set; }
    public int SimulationStartYear { get; set; } = DateTime.Now.Year;
    public int SimulationEndYear { get; set; }
    public decimal GeneralInflationRate { get; set; }
    public decimal HealthcareInflationRate { get; set; }

    public List<Person> People { get; set; } = [];
    public List<Investment> Investments { get; set; } = [];
    public List<Income> Incomes { get; set; } = [];
    public List<Expense> Expenses { get; set; } = [];
    public List<Asset> Assets { get; set; } = [];
    public List<TaxBracket> FederalTaxBrackets { get; set; } = [];
    public List<TaxBracket> StateTaxBrackets { get; set; } = [];
    public List<InvestmentRollover> InvestmentRollovers { get; set; } = [];
    public List<SurplusAllocationConfig> SurplusAllocations { get; set; } = [];
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly BirthDate { get; set; }
}

public class Investment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public InvestmentType InvestmentType { get; set; }
    public decimal CurrentBalance { get; set; }

    public List<InvestmentDistributionConfig> DistributionConfig { get; set; } = [];

    public decimal ExpectedDividendYield { get; set; }
    public decimal ExpectedGrowthRate { get; set; }
    public decimal ExpectedReturnOfCapitalRate { get; set; }
    public decimal ExpenseRatio { get; set; }
    public decimal ExpectedTurnoverRate { get; set; }

    public bool IsFederalTaxExempt { get; set; }
    public bool IsStateTaxExempt { get; set; }

    public int WithdrawalPriority { get; set; }
    public bool RequiresRMD { get; set; }
    public int? RMDStartAge { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; }
}

public class InvestmentDistributionConfig
{
    public int Id { get; set; }
    public DistributionCategory Category { get; set; }
    public decimal AnnualPercentage { get; set; }
    public int InvestmentId { get; set; }
    public Investment Investment { get; set; }
}

public class Income
{
    public int Id { get; set; }
    public string Source { get; set; }
    public IncomeType IncomeType { get; set; }
    public decimal AnnualAmount { get; set; }
    public bool IsTaxExempt { get; set; }
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public int? PersonId { get; set; }
    public Person? Person { get; set; }
}

public class Expense
{
    public int Id { get; set; }
    public string Category { get; set; }
    public decimal AnnualAmount { get; set; }
    public bool IsHealthcare { get; set; }
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public string Notes { get; set; }
}

public class Asset
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public bool IsLiquid { get; set; }
    public bool IsPrimaryResidence { get; set; }
    public bool IsDepreciable { get; set; }
    public decimal? AppreciationRate { get; set; }
}

public class TaxBracket
{
    public int Id { get; set; }
    public TaxType TaxType { get; set; }
    public FilingStatus FilingStatus { get; set; }
    public decimal MinIncome { get; set; }
    public decimal MaxIncome { get; set; }
    public decimal Rate { get; set; }
    public decimal? Threshold { get; set; }
    public string? State { get; set; }
}

public class InvestmentRollover
{
    public int Id { get; set; }
    public int SourceInvestmentId { get; set; }
    public Investment SourceInvestment { get; set; }
    public int DestinationInvestmentId { get; set; }
    public Investment DestinationInvestment { get; set; }
    public decimal Amount { get; set; }
    public int Year { get; set; }
    public RolloverType RolloverType { get; set; }
}

public class SurplusAllocationConfig
{
    public int Id { get; set; }
    public int InvestmentId { get; set; }
    public Investment Investment { get; set; }
    public decimal AllocationPercentage { get; set; }
}
