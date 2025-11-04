using PinoyCleanArch.Domain.Common.Models;
using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Domain.TaskAggregate.Events;

public sealed class TaskCompleted : IDomainEvent
{
    public TaskId AggregateId { get; }
    public DateTimeOffset CompletionTime { get; }
    public DateTimeOffset Timestamp { get; }

    public TaskCompleted(TaskId aggregateId)
    {
        AggregateId = aggregateId;
        CompletionTime = DateTimeOffset.UtcNow;
        Timestamp = DateTimeOffset.UtcNow;
    }
}