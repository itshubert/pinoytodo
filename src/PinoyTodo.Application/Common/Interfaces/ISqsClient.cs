namespace PinoyTodo.Application.Common.Interfaces;

public interface ISqsClient
{
    Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
        where TMessage : class;
}