using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Application.Common.Interfaces;

public interface ITaskRepository : IRepository
{
    Task<Domain.TaskAggregate.Task> Load(TaskId taskId, CancellationToken cancellationToken = default);
    Task Save(Domain.TaskAggregate.Task task, CancellationToken cancellationToken = default);
}