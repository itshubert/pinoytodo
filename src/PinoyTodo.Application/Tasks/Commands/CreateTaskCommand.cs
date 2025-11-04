using MediatR;
using PinoyTodo.Application.Common.Interfaces;

namespace PinoyTodo.Application.Tasks.Commands;

public sealed record CreateTaskCommand(string Title) : IRequest<Guid>;

public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new Domain.TaskAggregate.Task(request.Title);
        await _taskRepository.SaveAsync(task, cancellationToken);

        return task.Id.Value;
    }
}