# RetirementApp-Layout.md

## Purpose

Generate the shared layout, navigation, and reusable UI for the Retirement Planner Blazor app.  
All shared components and the main site layout (e.g., `MainLayout.razor`, `NavMenu.razor`, Toast/log areas) should be specified here.

---

## Tasks

1. Generate a responsive, Tailwind v4-styled `MainLayout.razor` component with:
    - A navigation sidebar or top nav that links to all CRUD pages, scenario/simulation, and home/dashboard.
    - A placeholder for toast notifications and/or a simple log panel.
    - Support for mobile-friendly navigation (collapsible or hamburger on small screens).
    - Use only Tailwind utility classes for all layout and styling (no custom CSS except variables/colors as configured in Tailwind).
    - Accessibility: proper aria-labels and keyboard navigation for menus.

2. Define any shared UI components used throughout the app:
    - Example: `Toast.razor`, `LogViewer.razor`, `Modal.razor`, or reusable card/form/layout wrappers as needed.
    - All components use Tailwind v4 utility classes.

3. Specify layout conventions:
    - App content always wrapped in a central container or flex/grid.
    - Consistent use of background, border, and text color utility classes as defined in your Tailwind config.

4. Place all layout and shared components in `/Shared` or `/Components/Shared` as per project structure.

---

## Output Format

- Output each file as a separate markdown code block with its filename.
- Components must use standard Blazor conventions.
- No business logic in layout—just presentational.

---

## Out of Scope

- Do not implement page-specific content here.
- Do not add JavaScript except for native Blazor interactions.

---

## Next Step

The following agent.md file will cover full developer build/run/migration instructions.

---

## End of File
