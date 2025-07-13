# RetirementApp-Models.md

## Purpose

Define all C# domain entities and enums for the Retirement Planner application, **using the exact model structure and property names provided below.**  
No code generation in this file—only property/type/relationship specs.

---

## Enums

- **FilingStatus:** Single, MarriedFilingJointly, MarriedFilingSeparately, HeadOfHousehold, QualifyingWidow
- **InvestmentType:** TaxableBrokerage, TraditionalIRA, RothIRA, Employer401k, Roth401k, HSA, Annuity, Pension
- **IncomeType:** W2, SelfEmployment, TaxExempt, Pension, SocialSecurity, OtherTaxable
- **DistributionCategory:** Interest, QualifiedDividend, NonQualifiedDividend, ShortTermCapitalGain, LongTermCapitalGain, ReturnOfCapital, Other
- **TaxType:** OrdinaryIncome, QualifiedDividend, ShortTermCapitalGain, LongTermCapitalGain, NetInvestmentIncomeTax, AdditionalMedicareTax, StateOrdinaryIncome, StateCapitalGains
- **RolloverType:** RothConversion, IraToIra, EmployerToIra, Other

---

## Entities

### RetirementPlannerContext  
*(Application-wide context for scenario/simulation runs)*

- FilingStatus (FilingStatus enum)
- State (string)
- SimulationStartYear (int, defaults to current year)
- SimulationEndYear (int)
- GeneralInflationRate (decimal)
- HealthcareInflationRate (decimal)
- People (List<Person>)
- Investments (List<Investment>)
- Incomes (List<Income>)
- Expenses (List<Expense>)
- Assets (List<Asset>)
- FederalTaxBrackets (List<TaxBracket>)
- StateTaxBrackets (List<TaxBracket>)
- InvestmentRollovers (List<InvestmentRollover>)
- SurplusAllocations (List<SurplusAllocationConfig>)

---

### Person

- Id (int, PK)
- Name (string)
- BirthDate (DateOnly)

---

### Investment

- Id (int, PK)
- Name (string)
- InvestmentType (InvestmentType enum)
- CurrentBalance (decimal)
- DistributionConfig (List<InvestmentDistributionConfig>)
- ExpectedDividendYield (decimal)
- ExpectedGrowthRate (decimal)
- ExpectedReturnOfCapitalRate (decimal)
- ExpenseRatio (decimal)
- ExpectedTurnoverRate (decimal)
- IsFederalTaxExempt (bool)
- IsStateTaxExempt (bool)
- WithdrawalPriority (int)
- RequiresRMD (bool)
- RMDStartAge (int?, nullable)
- PersonId (int, FK)
- Person (Person)

---

### InvestmentDistributionConfig

- Id (int, PK)
- Category (DistributionCategory enum)
- AnnualPercentage (decimal)
- InvestmentId (int, FK)
- Investment (Investment)

---

### Income

- Id (int, PK)
- Source (string)
- IncomeType (IncomeType enum)
- AnnualAmount (decimal)
- IsTaxExempt (bool)
- StartYear (int)
- EndYear (int?, nullable)
- PersonId (int?, nullable, FK)
- Person (Person?, nullable)

---

### Expense

- Id (int, PK)
- Category (string)
- AnnualAmount (decimal)
- IsHealthcare (bool)
- StartYear (int)
- EndYear (int?, nullable)
- Notes (string)

---

### Asset

- Id (int, PK)
- Name (string)
- Value (decimal)
- IsLiquid (bool)
- IsPrimaryResidence (bool)
- IsDepreciable (bool)
- AppreciationRate (decimal?, nullable)

---

### TaxBracket

- Id (int, PK)
- TaxType (TaxType enum)
- FilingStatus (FilingStatus enum)
- MinIncome (decimal)
- MaxIncome (decimal)
- Rate (decimal)
- Threshold (decimal?, nullable)
- State (string?, nullable)

---

### InvestmentRollover

- Id (int, PK)
- SourceInvestmentId (int, FK)
- SourceInvestment (Investment)
- DestinationInvestmentId (int, FK)
- DestinationInvestment (Investment)
- Amount (decimal)
- Year (int)
- RolloverType (RolloverType enum)

---

### SurplusAllocationConfig

- Id (int, PK)
- InvestmentId (int, FK)
- Investment (Investment)
- AllocationPercentage (decimal)

---

## Model Standards

- Use these exact property names, types, and relationships for all future code generation.
- EF Core annotations should be added for PK, FK, and navigation properties.
- All enums must be represented as C# enums.
- Use nullable types where indicated.
- Lists should be mapped as navigation properties.

---

## Out of Scope

- No code, scaffolding, or migration generation in this file.
- No UI or service design here.
- No alternate naming or extra fields.

---

## Next Step

The next agent.md file will generate the C# code for all enums and model classes as specified here, with EF Core attributes and relationships.

---

## End of File
