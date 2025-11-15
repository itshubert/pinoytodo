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

    public void Complete()
    {
        if (!IsCompleted)
        {
            Apply(new TaskCompleted(Id));
        }
    }

    public void UpdateTitle(string newTitle)
    {
        if (Title != newTitle)
        {
            var oldTitle = Title;
            Title = newTitle;
            Apply(new TaskTitleUpdated(Id, oldTitle, newTitle));
        }
    }

    public void Delete()
    {
        Apply(new TaskDeleted(Id));
    }

    private void Apply(IDomainEvent e, bool isNew = true)
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
            case TaskTitleUpdated titleUpdated:
                Title = titleUpdated.NewTitle;
                break;
            case TaskDeleted:
                // Handle deletion logic if necessary
                break;
            default:
                throw new InvalidOperationException("Unknown domain event");
        }

        if (isNew)
        {
            AddDomainEvent(e);
        }
    }

}