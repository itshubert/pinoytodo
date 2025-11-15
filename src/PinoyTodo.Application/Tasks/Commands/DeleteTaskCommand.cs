using ErrorOr;
using MediatR;
using PinoyTodo.Application.Common.Interfaces;
using PinoyTodo.Domain.Common.Errors;
using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Application.Tasks.Commands;

public sealed record DeleteTaskCommand(Guid TaskId) : IRequest<ErrorOr<Unit>>;

public sealed class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, ErrorOr<Unit>>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.LoadAsync(TaskId.Create(request.TaskId), cancellationToken);

        if (task is null)
        {
            return Errors.Task.InvalidTaskId(request.TaskId);
        }

        task.Delete();

        await _taskRepository.SaveAsync(task, cancellationToken);

        return Unit.Value;
    }
}