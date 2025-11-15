using ErrorOr;

namespace PinoyTodo.Domain.Common.Errors;

public static partial class Errors
{
    public static class Task
    {
        public static Error NotFound => Error.NotFound(
            code: "Task.NotFound",
            description: "The specified task was not found.");

        public static Error InvalidTaskId(Guid taskId) => Error.Validation(
            code: "Task.InvalidTaskId",
            description: $"The task with Task ID '{taskId}' is invalid or does not exist.");
    }
}