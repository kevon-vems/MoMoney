using System.Linq;
using Microsoft.EntityFrameworkCore;
using RetirementPlanner.Models;

namespace RetirementPlanner.Data;

public class RetirementPlannerContext : DbContext
{
    public RetirementPlannerContext(DbContextOptions<RetirementPlannerContext> options)
        : base(options)
    {
    }

    public DbSet<Person> People => Set<Person>();
    public DbSet<Investment> Investments => Set<Investment>();
    public DbSet<InvestmentDistributionConfig> InvestmentDistributionConfigs => Set<InvestmentDistributionConfig>();
    public DbSet<Income> Incomes => Set<Income>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<TaxBracket> TaxBrackets => Set<TaxBracket>();
    public DbSet<InvestmentRollover> InvestmentRollovers => Set<InvestmentRollover>();
    public DbSet<SurplusAllocationConfig> SurplusAllocations => Set<SurplusAllocationConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties().Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            foreach (var property in entity.GetProperties().Where(p => p.ClrType == typeof(string)))
            {
                if (property.GetMaxLength() == null)
                {
                    property.SetMaxLength(100);
                }
            }
        }

        modelBuilder.Entity<Person>()
            .Property(p => p.BirthDate)
            .HasColumnType("date");

        modelBuilder.Entity<Investment>()
            .Property(i => i.InvestmentType)
            .HasConversion<string>();

        modelBuilder.Entity<InvestmentDistributionConfig>()
            .Property(d => d.Category)
            .HasConversion<string>();

        modelBuilder.Entity<Income>()
            .Property(i => i.IncomeType)
            .HasConversion<string>();

        modelBuilder.Entity<TaxBracket>()
            .Property(t => t.TaxType)
            .HasConversion<string>();

        modelBuilder.Entity<TaxBracket>()
            .Property(t => t.FilingStatus)
            .HasConversion<string>();

        modelBuilder.Entity<InvestmentRollover>()
            .Property(r => r.RolloverType)
            .HasConversion<string>();

        modelBuilder.Entity<InvestmentRollover>()
            .HasOne(r => r.SourceInvestment)
            .WithMany()
            .HasForeignKey(r => r.SourceInvestmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InvestmentRollover>()
            .HasOne(r => r.DestinationInvestment)
            .WithMany()
            .HasForeignKey(r => r.DestinationInvestmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
