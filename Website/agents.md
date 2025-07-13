# RetirementApp-Initial.md

## Purpose

This file defines the master specification for the Retirement Planner web application project.  
It exists to coordinate, sequence, and standardize all subsequent agent.md files required to generate a production-grade .NET 9/Blazor/EF Core web application for multi-scenario retirement modeling and simulation.

---

## Project Scope & Goals

- Build a self-hosted, no-login, responsive web app for detailed, scenario-based retirement financial planning.
- Enable users to define investments, people, assets, expenses, tax settings, and simulation parameters.
- Support saving, editing, and re-running scenarios, with interactive visualizations of results.
- Everything must be modular, maintainable, and extensible for future authentication, multi-user, or API features.

---

## Technical Constraints

- .NET 9, C#, Blazor (Server)
- Entity Framework Core 9 for persistence (SQL Server)
- Tailwind CSS v4 for UI styling (no other CSS frameworks)
- All code in English, all UI in English
- Do not generate seed/sample data in this step

---

## Key Entities (High Level Only)

1. **Person** — name, birth date, etc.
2. **Investment** — type, balance, distribution configs
3. **Expense** — type, schedule, annual amounts
4. **Asset** — value, category
5. **TaxBracket** — filing status, min/max/rate
6. **Income** — source, amount, timing
7. **SimulationScenario** — links to all above for runs
8. **InvestmentRollover** — for modeling Roth conversions/etc.
9. **SurplusAllocationConfig** — rules for handling simulation surplus

*(Do NOT define properties here; detail is handled by later agent.md files.)*

---

## User Stories (Summarized)

- As a user, I can define multiple people, investments, assets, expenses, and tax settings.
- As a user, I can create, save, edit, clone, and delete simulation scenarios.
- As a user, I can run a scenario and view time series charts (portfolio, income, expenses).
- As a user, I can download simulation results as Excel.

---

## Architecture & Standards

- Each logical concern (models, db context, services, pages, UI, config, instructions) must be specified in its own agent.md file.
- Use strong typing and data annotations throughout.
- CRUD UIs for all user-editable entities.
- Visualizations must use open-source Blazor charting.
- No business logic in Razor pages; services handle all calculations.
- UI must be responsive and accessible.

---

## Development Workflow

1. **This file (RetirementApp-Initial.md)** is processed first.
2. It is followed by a sequenced set of agent.md files (see below), each responsible for only its scope.
3. Each subsequent agent.md must reference and respect decisions made in this master file.
4. If a requirement is ambiguous, subsequent agent.md files must request clarification.

---

## Naming Conventions

- All class, file, and property names must use PascalCase.
- Entity files go in `/Models`, services in `/Services`, pages in `/Pages`, components in `/Components`, context in `/Data`.
- No underscores in file/class/property names.
- All subsequent agent.md files should use this naming and structure.

---

## Agent.md Sequence

1. **RetirementApp-Models.md**: All C# entity definitions with EF Core annotations.
2. **RetirementApp-Context.md**: DbContext for all entities.
3. **RetirementApp-Services.md**: Data access and business logic services.
4. **RetirementApp-Pages.md**: Blazor pages for CRUD and simulation.
5. **RetirementApp-Visualization.md**: Charts and visual components.
6. **RetirementApp-Layout.md**: Main layout, nav, shared UI.
7. **RetirementApp-Tailwind.md**: Tailwind config and usage.
8. **RetirementApp-Instructions.md**: Build, migration, run instructions.

---

## Cross-Cutting Requirements

- All validation must be enforced both at the UI and data model level.
- All user input must be sanitized.
- App must display clear error/success notifications.
- Architecture must support future extension for login, API, etc.

---

## Out of Scope for This File

- Do NOT generate any code, scaffolding, or markup.
- Do NOT define property-level details for entities.
- Do NOT generate seed/sample data or connection strings.

---

## Output Format

- Markdown file only, structured as above.
- No code, only project-wide decisions and sequencing.

---

## End of File
