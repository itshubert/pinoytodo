using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Application.Common.Interfaces;

namespace PinoyTodo.Application.Tasks.EventHandlers;

public sealed class TaskDeletedDomainEventHandler : INotificationHandler<Domain.TaskAggregate.Events.TaskDeleted>
{
    private readonly ISqsClient _sqsClient;
    private readonly ILogger<TaskDeletedDomainEventHandler> _logger;

    public TaskDeletedDomainEventHandler(ISqsClient sqsClient, ILogger<TaskDeletedDomainEventHandler> logger)
    {
        _sqsClient = sqsClient;
        _logger = logger;
    }

    public async Task Handle(Domain.TaskAggregate.Events.TaskDeleted notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling TaskDeleted domain event for TaskId: {TaskId}", notification.AggregateId);
        await _sqsClient.SendAsync(notification, cancellationToken);
    }
}