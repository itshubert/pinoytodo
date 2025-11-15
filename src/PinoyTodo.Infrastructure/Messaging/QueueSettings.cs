namespace PinoyTodo.Infrastructure.Messaging;

public sealed class QueueSettings
{
    public string TaskCreatedQueueName { get; set; } = "task-created-queue";
    public string TaskCompletedQueueName { get; set; } = "task-completed-queue";
    public string TaskTitleUpdatedQueueName { get; set; } = "task-title-updated-queue";
    public string TaskDeletedQueueName { get; set; } = "task-deleted-queue";
}