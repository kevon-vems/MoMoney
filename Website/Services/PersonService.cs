using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Data;
using RetirementPlanner.Models;

namespace RetirementPlanner.Services;

/// <summary>
/// EF Core backed implementation of <see cref="IPersonService"/>.
/// </summary>
public class PersonService : IPersonService
{
    private readonly RetirementPlannerContext _context;

    public PersonService(RetirementPlannerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieve all <see cref="Person"/> records.
    /// </summary>
    public async Task<List<Person>> GetAllAsync()
    {
        return await _context.People.ToListAsync();
    }

    /// <summary>
    /// Get a single <see cref="Person"/> by identifier.
    /// </summary>
    public async Task<Person?> GetByIdAsync(int id)
    {
        return await _context.People.FindAsync(id);
    }

    /// <summary>
    /// Add a new <see cref="Person"/>.
    /// </summary>
    public async Task<Person> AddAsync(Person entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.People.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Update an existing <see cref="Person"/>.
    /// </summary>
    public async Task UpdateAsync(Person entity)
    {
        Validator.ValidateObject(entity, new ValidationContext(entity), true);
        _context.People.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Delete the <see cref="Person"/> with the given identifier.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var existing = await _context.People.FindAsync(id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Person {id} not found");
        }

        _context.People.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
