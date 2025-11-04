using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Application.Common.Interfaces;

public interface ITaskRepository : IRepository
{
    Task<Domain.TaskAggregate.Task> LoadAsync(TaskId taskId, CancellationToken cancellationToken = default);
    Task SaveAsync(Domain.TaskAggregate.Task task, CancellationToken cancellationToken = default);
}