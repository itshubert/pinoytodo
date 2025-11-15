using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PinoyCleanArch.Domain.Common.Models;
using PinoyTodo.Application.Common.Interfaces;
using PinoyTodo.Domain.TaskAggregate.ValueObjects;
using PinoyTodo.Infrastructure.Persistence.Models;

namespace PinoyTodo.Infrastructure.Persistence.Repositories;


public sealed class TaskRepository : BaseRepository, ITaskRepository
{
    public TaskRepository(EventStoreDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        await Context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        await Context.Database.CommitTransactionAsync();
    }

    public async Task<Domain.TaskAggregate.Task?> LoadAsync(TaskId taskId, CancellationToken cancellationToken = default)
    {
        var events = await Context.StoredEvents
            .Where(se => se.AggregateId == taskId.Value)
            .OrderBy(se => se.Version)
            .ToListAsync(cancellationToken);

        if (!events.Any())
        {
            return null;
        }

        var result = events.Select(se =>
        {
            var eventType = Type.GetType(se.EventType) ?? throw new InvalidOperationException($"Unknown event type: {se.EventType}");
            var data = JsonSerializer.Deserialize(se.EventData, eventType) ?? throw new InvalidOperationException($"Failed to deserialize event data for type: {se.EventType}");

            return (IDomainEvent)data;
        });

        return new Domain.TaskAggregate.Task(result);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync(Domain.TaskAggregate.Task task, CancellationToken cancellationToken = default)
    {
        AddStoreEvents(task);

        await Context.SaveChangesAsync(cancellationToken);
        task.ClearDomainEvents();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }

    private void AddStoreEvents(Domain.TaskAggregate.Task task)
    {
        var newEvents = task.DomainEvents.ToList();
        if (!newEvents.Any())
        {
            return;
        }

        foreach (var e in newEvents)
        {
            var storedEvent = new StoredEvent
            {
                AggregateId = task.Id.Value,
                EventType = e.GetType().AssemblyQualifiedName ?? throw new InvalidOperationException("Event type cannot be determined."),
                EventData = JsonSerializer.Serialize(e, e.GetType()),
                Version = task.Version,
                Timestamp = e.Timestamp
            };
            storedEvent.AddDomainEvent(e);

            Context.StoredEvents.Add(storedEvent);
        }
    }
}