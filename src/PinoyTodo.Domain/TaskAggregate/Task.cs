using PinoyCleanArch;
using PinoyCleanArch.Domain.Common.Models;
using PinoyTodo.Domain.TaskAggregate.Events;
using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Domain.TaskAggregate;

public sealed class Task : AggregateRoot<TaskId>
{
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTimeOffset? CompletionTime { get; private set; }
    public int Version { get; private set; }

    public Task(string title)
    {
        Title = title;
        Apply(new TaskCreated(TaskId.CreateUnique(), title), true);
    }

    public Task(IEnumerable<IDomainEvent> events)
    {
        Title = string.Empty;
        foreach (var e in events)
        {
            Apply(e, false);
        }
    }

    private void Apply(IDomainEvent e, bool isNew)
    {
        switch (e)
        {
            case TaskCreated created:
                Id = created.AggregateId;
                Title = created.Title;
                IsCompleted = false;
                break;
            case TaskCompleted completed:
                IsCompleted = true;
                CompletionTime = completed.CompletionTime;
                break;
        }

        if (isNew)
        {
            AddDomainEvent(e);
        }
    }

}