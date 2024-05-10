using AdventureGuardian.Models.Models;
using AdventureGuardian.Models.Models.Domain;
using AdventureGuardian.Models.Models.Domain.Worlds;
using Microsoft.EntityFrameworkCore;

namespace AdventureGuardian.Infrastructure.Persistance;

public class AdventureGuardianDbContext : DbContext
{
    public DbSet<Campaign> Campaigns { get; set; } = null!;
    public DbSet<World> Worlds { get; set; } = null!;
    public DbSet<Character> Characters { get; set; } = null!;
    public DbSet<Encounter> Encounters { get; set; } = null!;
    public DbSet<Stats> Stats { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5436;Database=AdventureGuardian;Username=postgres;Password=password");
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<FantasyWorld>();
        builder.Entity<SciFiWorld>();
        builder.Entity<RealismWorld>();

        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}