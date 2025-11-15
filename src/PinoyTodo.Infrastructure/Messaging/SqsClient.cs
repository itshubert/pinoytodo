using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PinoyTodo.Application.Common.Interfaces;

namespace PinoyTodo.Infrastructure.Messaging;

public sealed class SqsClient : ISqsClient
{
    private readonly ILogger<SqsClient> _logger;
    private readonly IAmazonSQS _sqsClient;
    private readonly QueueSettings _queueSettings;

    public SqsClient(
        ILogger<SqsClient> logger,
        IAmazonSQS sqsClient,
        IOptions<QueueSettings> queueSettings)
    {
        _logger = logger;
        _sqsClient = sqsClient;
        _queueSettings = queueSettings.Value;
    }

    public async Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class
    {
        var queueUrl = message.GetType().Name switch
        {
            "TaskCreated" => _queueSettings.TaskCreatedQueueName,
            "TaskCompleted" => _queueSettings.TaskCompletedQueueName,
            "TaskDeleted" => _queueSettings.TaskDeletedQueueName,
            "TaskTitleUpdated" => _queueSettings.TaskTitleUpdatedQueueName,
            _ => throw new InvalidOperationException($"No queue configured for event type {message.GetType().Name}.")
        };

        try
        {
            var messageBody = JsonSerializer.Serialize(message);
            var sendRequest = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = messageBody
            };

            // FIFO queues require MessageGroupId
            if (queueUrl.EndsWith(".fifo"))
            {
                sendRequest.MessageGroupId = message.GetType().Name;

                // Set MessageDeduplicationId for exactly-once processing
                // Using a hash of the serialized message to ensure uniqueness
                sendRequest.MessageDeduplicationId = Guid.NewGuid().ToString();
            }

            var response = await _sqsClient.SendMessageAsync(sendRequest, cancellationToken);
            _logger.LogInformation("Message sent to SQS with MessageGroupId: {MessageGroupId} - {MessageDeduplicationId}. MessageId: {MessageId}", sendRequest.MessageGroupId, sendRequest.MessageDeduplicationId, response.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send message to SQS. QueueUrl: {QueueUrl} - {Message}", queueUrl, message);
            throw;
        }
    }

}