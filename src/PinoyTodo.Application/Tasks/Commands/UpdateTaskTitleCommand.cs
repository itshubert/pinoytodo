using ErrorOr;
using MediatR;
using PinoyTodo.Application.Common.Interfaces;
using PinoyTodo.Domain.Common.Errors;
using PinoyTodo.Domain.TaskAggregate.ValueObjects;

namespace PinoyTodo.Application.Tasks.Commands;

public sealed record UpdateTaskTitleCommand(Guid TaskId, string NewTitle) : IRequest<ErrorOr<Unit>>;

public sealed class UpdateTaskTitleCommandHandler : IRequestHandler<UpdateTaskTitleCommand, ErrorOr<Unit>>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskTitleCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateTaskTitleCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.LoadAsync(TaskId.Create(request.TaskId), cancellationToken);

        if (task is null)
        {
            return Errors.Task.InvalidTaskId(request.TaskId);
        }

        task.UpdateTitle(request.NewTitle);

        await _taskRepository.SaveAsync(task, cancellationToken);

        return Unit.Value;
    }
}