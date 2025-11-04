using MediatR;
using PinoyTodo.Application.Common.Interfaces;
using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Application.Tasks.Commands;

public sealed record CompleteTaskCommand(Guid TaskId) : IRequest<Unit>;

public sealed class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, Unit>
{
    private readonly ITaskRepository _taskRepository;

    public CompleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Unit> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.LoadAsync(TaskId.Create(request.TaskId), cancellationToken);
        task.Complete();

        await _taskRepository.SaveAsync(task, cancellationToken);

        return Unit.Value;
    }
}