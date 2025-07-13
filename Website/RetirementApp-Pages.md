# RetirementApp-Pages.md

## Purpose

Generate Blazor pages and supporting components for CRUD, scenario management, and simulation results visualization in the Retirement Planner app.

---

## Input

- All services and interfaces from RetirementApp-Services.md.
- Entity and enum models from RetirementApp-Models-CodeGen.md.
- Use the design and relationships from RetirementApp-Models.md.

---

## Tasks

1. For **each entity** (`Person`, `Investment`, `Income`, `Expense`, `Asset`, `TaxBracket`, `InvestmentRollover`, `SurplusAllocationConfig`), generate:
    - A Blazor CRUD page (Razor file) at route `/[entity]s` (e.g., `/people`, `/investments`).
    - Each page must:
        - Display all records in a responsive Tailwind-styled table.
        - Provide buttons/links for Create, Edit, and Delete.
        - Use modal dialogs or side panels (not inline forms) for add/edit actions.
        - Use data validation (required fields, enum drop-downs, numeric ranges, etc).
        - Bind data via the appropriate service using dependency injection.
        - Show error/success notifications as Toasts.
        - Support full async load/save.
        - Use only built-in Blazor features and Tailwind classes.

2. For **scenario management and simulation**:
    - Create a `/scenarios` page that:
        - Lets the user create, clone, edit, and delete simulation scenarios.
        - Lets the user select scenario parameters (years, inflation, filing status, etc).
        - Displays summary info for each scenario.
    - Create a `/simulate/{scenarioId}` page that:
        - Runs the selected scenario using the configured models and services.
        - Displays results as interactive charts (line chart for portfolio value, bar/area charts for income/expense).
        - Shows year-by-year cashflows and major events (rollovers, RMDs).
        - Allows export/download of results as Excel.

3. For **shared layout/navigation**:
    - Create or update a MainLayout.razor that includes:
        - Responsive navigation menu with links to all CRUD pages and scenarios.
        - User feedback/log/toast area.
        - Light/dark mode toggle (optional).

4. All pages must use English text, Tailwind CSS, and be responsive/mobile friendly.

5. Do **not** generate any API controllers or JavaScript—Blazor/C# only.

---

## Output Format

- Output each page and component as a separate markdown code block, with clear file names.
- Razor code should include @page directives, minimal code-behind (either inline or as partials).
- All components/pages use dependency injection for services.
- Include using statements as needed.

---

## Out of Scope

- Do not generate seed/sample data.
- Do not generate the underlying services or models here.
- Do not generate database context or migrations.
- Do not include API endpoints.

---

## Next Step

The following agent.md file will cover Tailwind CSS configuration and usage instructions for the Blazor project.

---

## End of File
