using PinoyCleanArch.Domain.Common.Models;

namespace PinoyTodo.Domain.TaskAggregate.ValueObjects;

public sealed class TaskId : ValueObject
{
    public Guid Value { get; }
    public TaskId(Guid value)
    {
        Value = value;
    }

    public static TaskId CreateUnique() => new(Guid.NewGuid());

    public static TaskId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

#pragma warning disable CS8618 // for EF Core
    private TaskId() { }
#pragma warning restore CS8618
}