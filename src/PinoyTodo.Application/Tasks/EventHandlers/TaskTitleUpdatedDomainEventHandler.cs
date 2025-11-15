using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Application.Common.Interfaces;

namespace PinoyTodo.Application.Tasks.EventHandlers;

public sealed class TaskTitleUpdatedDomainEventHandler : INotificationHandler<Domain.TaskAggregate.Events.TaskTitleUpdated>
{
    private readonly ISqsClient _sqsClient;
    private readonly ILogger<TaskTitleUpdatedDomainEventHandler> _logger;

    public TaskTitleUpdatedDomainEventHandler(
        ISqsClient sqsClient,
        ILogger<TaskTitleUpdatedDomainEventHandler> logger)
    {
        _sqsClient = sqsClient;
        _logger = logger;
    }

    public async Task Handle(Domain.TaskAggregate.Events.TaskTitleUpdated notification, CancellationToken cancellationToken)
    {
        await _sqsClient.SendAsync(notification, cancellationToken);
    }
}