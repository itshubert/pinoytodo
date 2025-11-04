
using Microsoft.EntityFrameworkCore;
using PinoyCleanArch.Infrastructure.Interceptors;
using PinoyTodo.Infrastructure.Persistence.Models;

namespace PinoyTodo.Infrastructure.Persistence;

public class EventStoreDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor)
        : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<StoredEvent> StoredEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventStoreDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}