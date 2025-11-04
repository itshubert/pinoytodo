using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Application.Common.Interfaces;

namespace PinoyTodo.Application.Tasks.EventHandlers;

public sealed class TaskCompletedDomainEventHandler : INotificationHandler<Domain.TaskAggregate.Events.TaskCompleted>
{
    private readonly ISqsClient _sqsClient;
    private readonly ILogger<TaskCompletedDomainEventHandler> _logger;

    public TaskCompletedDomainEventHandler(
        ISqsClient sqsClient,
        ILogger<TaskCompletedDomainEventHandler> logger)
    {
        _sqsClient = sqsClient;
        _logger = logger;
    }

    public async Task Handle(Domain.TaskAggregate.Events.TaskCompleted notification, CancellationToken cancellationToken)
    {
        await _sqsClient.SendAsync(notification, cancellationToken);
    }
}