# RetirementApp-Services.md

## Purpose

Generate strongly-typed, testable data access services (repositories) for all entities in the Retirement Planner application.  
Services must encapsulate CRUD operations and any basic query logic, using the RetirementPlannerContext produced previously.

---

## Input

- The EF Core DbContext (`RetirementPlannerContext`) as generated in RetirementApp-Context.md.
- All entity and enum definitions in RetirementApp-Models-CodeGen.md.

---

## Tasks

1. For each entity, generate a service interface and implementation pair:
    - Interface: `I[EntityName]Service` (e.g., `IPersonService`)
    - Implementation: `[EntityName]Service` (e.g., `PersonService`)
2. Each service must provide:
    - `Task<List<TEntity>> GetAllAsync();`
    - `Task<TEntity?> GetByIdAsync(int id);`
    - `Task<TEntity> AddAsync(TEntity entity);`
    - `Task UpdateAsync(TEntity entity);`
    - `Task DeleteAsync(int id);`
3. All methods must use asynchronous programming (async/await).
4. Inject `RetirementPlannerContext` via constructor (use constructor injection, not service locator).
5. Apply best practices:
    - Validate required fields before database operations.
    - Throw meaningful exceptions if not found (or return null where appropriate).
    - For Add/Update, save changes before returning.
6. Place each interface and implementation in `RetirementPlanner.Services` namespace.
7. Add comments summarizing the purpose of each method and interface.
8. Output all services and interfaces as a single markdown code block, one file per service/interface pair.

---

## Output Format

- Each interface/implementation pair as one logical code section.
- All code in the `RetirementPlanner.Services` namespace.
- Use necessary using statements (`using RetirementPlanner.Models;`, `using RetirementPlanner.Data;`, etc.)
- Do **not** register services in DI in this file (comes later).
- Do **not** generate UI or API logic.

---

## Out of Scope

- Do not handle advanced business logic (only CRUD/query).
- Do not generate UI, Blazor pages, or API controllers.
- Do not generate DI registration code.

---

## Next Step

The following agent.md will generate Blazor UI pages/components for CRUD and scenario simulation/visualization.

---

## End of File
