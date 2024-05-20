using MessageGateway.Events;
using Microsoft.EntityFrameworkCore;

namespace ExternalCommunicator.Infrastructure;

public class ExternalCommunicatorDbContext : DbContext
{
    public DbSet<Event> Events { get; set; } = null!;

    // TODO: Remove this, use startup dependency injection instead
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=postgres;Port=5432;Database=ExternalCommunicator;Username=postgres;Password=password");
    }
}