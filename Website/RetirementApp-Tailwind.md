# RetirementApp-Tailwind.md

## Purpose

Specify streamlined Tailwind CSS v4 setup and usage for the Retirement Planner Blazor app.

---

## Tasks

1. **Install Tailwind v4**
    - Run:
      ```sh
      npm install tailwindcss @tailwindcss/postcss
      ```

2. **Add PostCSS Plugin**
    - In your `postcss.config.js`:
      ```js
      export default {
        plugins: ["@tailwindcss/postcss"],
      };
      ```

3. **Import Tailwind in CSS**
    - In your main CSS file (e.g., `wwwroot/css/app.css`):
      ```css
      @import "tailwindcss";
      ```

4. **(Optional) Customization**
    - Create `tailwind.config.js` (if customizing colors/utilities).
    - Add custom color tokens as needed under `extend > colors`.

5. **Build Tailwind**
    - Generate CSS with:
      ```sh
      npx tailwindcss -i ./wwwroot/css/app.css -o ./wwwroot/css/app.generated.css --watch
      ```

6. **Usage in Blazor**
    - Use Tailwind utility classes directly in all `.razor` files.
    - Reference the generated CSS in `_Host.cshtml` (Server) or `index.html` (WASM).

7. **Reference**
    - [Tailwind CSS Documentation](https://tailwindcss.com/docs/installation)

---

## Output Format

- Output each step as a list.
- Use code blocks for commands and config.

---

## End of File
