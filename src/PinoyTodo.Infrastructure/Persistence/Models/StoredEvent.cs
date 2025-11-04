namespace PinoyTodo.Infrastructure.Persistence.Models;

public sealed class StoredEvent
{
    public long Id { get; set; }
    public Guid AggregateId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty;
    public int Version { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}