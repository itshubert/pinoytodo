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

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Domain.TaskAggregate.Task> Load(TaskId taskId, CancellationToken cancellationToken = default)
    {
        var events = await Context.StoredEvents
            .Where(se => se.AggregateId == taskId.Value)
            .OrderBy(se => se.Version)
            .ToListAsync(cancellationToken);

        if (!events.Any())
        {
            throw new InvalidOperationException($"Task with ID {taskId} not found.");
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

    public async Task Save(Domain.TaskAggregate.Task task, CancellationToken cancellationToken = default)
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
                EventData = JsonSerializer.Serialize(e),
                Version = task.Version,
                Timestamp = e.Timestamp
            };

            Context.StoredEvents.Add(storedEvent);
        }

        await Context.SaveChangesAsync(cancellationToken);
        task.ClearDomainEvents();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}