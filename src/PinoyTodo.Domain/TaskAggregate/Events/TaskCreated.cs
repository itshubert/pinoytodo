using PinoyCleanArch.Domain.Common.Models;
using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Domain.TaskAggregate.Events;

public sealed class TaskCreated : IDomainEvent
{
    public TaskId AggregateId { get; }
    public string Title { get; }
    public DateTimeOffset Timestamp { get; }

    public TaskCreated(TaskId aggregateId, string title)
    {
        AggregateId = aggregateId;
        Title = title;
        Timestamp = DateTimeOffset.UtcNow;
    }
}