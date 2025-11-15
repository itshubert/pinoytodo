using PinoyCleanArch.Domain.Common.Models;
using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Domain.TaskAggregate.Events;

public sealed class TaskTitleUpdated : IDomainEvent
{
    public TaskId AggregateId { get; }
    public string OldTitle { get; }
    public string NewTitle { get; }
    public DateTimeOffset Timestamp { get; }

    public TaskTitleUpdated(TaskId aggregateId, string oldTitle, string newTitle)
    {
        AggregateId = aggregateId;
        OldTitle = oldTitle;
        NewTitle = newTitle;
        Timestamp = DateTimeOffset.UtcNow;
    }
}