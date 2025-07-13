# RetirementApp-Context.md

## Purpose

Generate the EF Core 9 DbContext (`RetirementPlannerContext`) for the Retirement Planner application, using the enums and entities defined in RetirementApp-Models-CodeGen.md.

---

## Input

All model/entity and enum class code produced in RetirementApp-Models-CodeGen.md.

---

## Tasks

1. Generate a C# class named `RetirementPlannerContext` that inherits from `DbContext` (in namespace `RetirementPlanner.Data`).
2. Add a `DbSet<T>` property for each model/entity class in the RetirementPlanner.Models namespace.
3. Implement a constructor that accepts `DbContextOptions<RetirementPlannerContext>`.
4. Override `OnModelCreating(ModelBuilder modelBuilder)` to:
    - Configure all entity relationships and navigation properties as required by the model specs.
    - Set decimal precision for all monetary fields (`decimal(18,2)`), including on nullable decimals.
    - Set max string lengths (default 100 if not specified).
    - Configure enum properties so they are stored as strings (using `.HasConversion<string>()`).
    - Configure collection navigations for one-to-many and many-to-many relationships.
    - Ensure `DateOnly` properties are mapped properly for SQL Server (use EF Core 9+ built-in support).
    - Apply `[Table("PluralName")]` where table names differ from class names.
    - Set up required/optional properties and nullability according to model specs.
    - Add any composite/alternate keys if required (but default to PKs from model).

5. Do **not** generate migration scripts or seed data in this file.

---

## Output Format

- Output a single code block containing the full context class, with using statements as needed.
- Place the class in the `RetirementPlanner.Data` namespace.
- Assume all models are in the `RetirementPlanner.Models` namespace and reference them accordingly.

---

## Out of Scope

- Do not generate enums or model/entity classes here.
- Do not generate any UI, services, or application logic.
- Do not create database or connection strings.
- Do not register the DbContext in DI (that comes later).

---

## Next Step

The next agent.md file will generate data services/repositories for CRUD and query access to each model.

---

## End of File
