
using Microsoft.EntityFrameworkCore;
using PinoyTodo.Infrastructure.Persistence.Models;

namespace PinoyTodo.Infrastructure.Persistence;

public class EventStoreDbContext : DbContext
{
    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<StoredEvent> StoredEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventStoreDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}