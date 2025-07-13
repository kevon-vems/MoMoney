# RetirementApp-Models-CodeGen.md

## Purpose

Generate complete, compile-ready C# source code for all enums and model/entity classes for the Retirement Planner app, using the specifications in RetirementApp-Models.md.  
Each class and enum must be a separate C# type definition, ready for direct use with EF Core 9.

---

## Input

Use the exact property names, types, and relationships specified in RetirementApp-Models.md.  
All enums and all entities must be included—no omissions or additions.

---

## Tasks

1. For each enum listed, generate a C# enum with all members.
2. For each entity listed, generate a C# class with:
   - All properties, using correct types and nullability.
   - `[Key]` on primary keys.
   - `[ForeignKey]` and navigation properties for relationships.
   - `[Required]` on all non-nullable properties except PKs.
   - `[StringLength]` on all string properties, with a default max length of 100 unless otherwise specified.
   - `[Column(TypeName = "decimal(18,2)")]` on all monetary/percentage fields.
   - All collection navigation properties as `public virtual ICollection<T> { get; set; } = new List<T>();`
   - Use `DateOnly` for date-only properties, and `DateTime` for date+time.
   - Add comments to each property describing its purpose if not obvious.
   - All enums should be included in the same output, in a `Models` namespace.

3. Do **not** generate the DbContext in this file.

---

## Output Format

- Output a single code block containing all enums and classes in proper C# syntax, with each type in its own file or region as appropriate for .NET conventions.
- All types must be placed in a `RetirementPlanner.Models` namespace.
- Use `using System.ComponentModel.DataAnnotations;` and `using System.ComponentModel.DataAnnotations.Schema;` as needed.

---

## Out of Scope

- Do not generate the RetirementPlannerContext (DbContext).
- Do not generate services, UI, or seed data.
- Do not generate migration scripts.

---

## Next Step

The following agent.md file will generate the RetirementPlannerContext and configure DbSets, relationships, and OnModelCreating overrides for EF Core 9.

---

## End of File
