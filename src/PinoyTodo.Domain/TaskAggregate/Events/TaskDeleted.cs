using PinoyCleanArch.Domain.Common.Models;
using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Domain.TaskAggregate.Events;

public sealed class TaskDeleted : IDomainEvent
{
    public TaskId AggregateId { get; }
    public DateTimeOffset Timestamp { get; }

    public TaskDeleted(TaskId aggregateId)
    {
        AggregateId = aggregateId;
        Timestamp = DateTimeOffset.UtcNow;
    }
}