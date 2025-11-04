using PinoyCleanArch.Domain.Common.Models;

namespace PinoyTodo.Infrastructure.Persistence.Models;

public sealed class StoredEvent : Entity<long>
{
    public Guid AggregateId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty;
    public int Version { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}