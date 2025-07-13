# RetirementApp-Instructions.md

## Purpose

Provide concise, up-to-date developer instructions for restoring, building, running, and maintaining the Retirement Planner Blazor app with .NET 9, EF Core 9, and Tailwind CSS v4.

---

## Tasks

1. **Restore and Build**
    - Output .NET CLI commands to restore and build the app.
    - Output npm commands to install and build Tailwind v4.

2. **EF Core Migrations**
    - Output .NET CLI commands for creating and applying EF Core migrations.
    - Include steps for updating the database after model changes.

3. **Tailwind CSS v4 Setup**
    - Install Tailwind CSS with:
      ```sh
      npm install tailwindcss @tailwindcss/postcss
      ```
    - In your main CSS file (e.g., `wwwroot/css/app.css`), add:
      ```css
      @import "tailwindcss";
      ```
    - (Optional) To customize colors, create a `tailwind.config.js` and use `extend`:
      ```js
      export default {
        theme: {
          extend: {
            colors: {
              brand: '#008cff',
              background: '#f8fafc',
              foreground: '#262626'
              // etc.
            },
          },
        },
        plugins: ["@tailwindcss/postcss"],
      };
      ```
    - Build Tailwind with:
      ```sh
      npx tailwindcss -i ./wwwroot/css/app.css -o ./wwwroot/css/app.generated.css --watch
      ```
    - Ensure your Blazor app imports the generated CSS in `_Host.cshtml` or `index.html`.

4. **Run the Application**
    - Use:
      ```sh
      dotnet run
      ```
    - The app should launch at the configured localhost port.

5. **Location of Key Files**
    - Models: `/Models`
    - Services: `/Services`
    - UI Pages: `/Pages`
    - Shared Components/Layout: `/Components/Shared` or `/Shared`
    - Data Context: `/Data`
    - Tailwind CSS: `/wwwroot/css/app.css` (or similar)

6. **Testing**
    - (Optional) Run any test projects with:
      ```sh
      dotnet test
      ```
    - Confirm CRUD, scenario, and simulation features work.

---

## Output Format

- Each step as a numbered or bulleted list.
- Use code blocks for commands and config.

---

## Out of Scope

- No seed/sample data.
- No custom JavaScript or third-party CSS frameworks.

---

## End of File
