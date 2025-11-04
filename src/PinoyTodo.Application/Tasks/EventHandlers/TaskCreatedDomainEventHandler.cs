using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Application.Common.Interfaces;

namespace PinoyTodo.Application.Tasks.EventHandlers;

public sealed class TaskCreatedDomainEventHandler : INotificationHandler<Domain.TaskAggregate.Events.TaskCreated>
{
    private readonly ISqsClient _sqsClient;
    private readonly ILogger<TaskCreatedDomainEventHandler> _logger;

    public TaskCreatedDomainEventHandler(ISqsClient sqsClient, ILogger<TaskCreatedDomainEventHandler> logger)
    {
        _sqsClient = sqsClient;
        _logger = logger;
    }

    public async Task Handle(Domain.TaskAggregate.Events.TaskCreated notification, CancellationToken cancellationToken)
    {
        await _sqsClient.SendAsync(notification, cancellationToken);
    }
}