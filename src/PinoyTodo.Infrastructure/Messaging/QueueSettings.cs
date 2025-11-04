namespace PinoyTodo.Infrastructure.Messaging;

public sealed class QueueSettings
{
    public string TaskCreatedQueueName { get; set; } = "task-created-queue";
    public string TaskCompletedQueueName { get; set; } = "task-completed-queue";
}