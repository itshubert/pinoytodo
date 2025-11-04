using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PinoyTodo.Infrastructure.Persistence.Models;

namespace PinoyTodo.Infrastructure.Persistence.Configurations;

public sealed class StoredEventConfiguration : IEntityTypeConfiguration<StoredEvent>
{
    public void Configure(EntityTypeBuilder<StoredEvent> builder)
    {
        builder.ToTable("StoredEvents");
        builder.HasKey(se => se.Id);

        builder.Property(se => se.Id)
            .ValueGeneratedOnAdd();

        builder.Property(se => se.AggregateId)
            .HasColumnName("aggregate_id")
            .IsRequired();

        builder.Property(se => se.EventType)
            .HasColumnName("event_type")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(se => se.EventData)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(se => se.Version)
            .HasColumnName("version")
            .IsRequired();

        builder.Property(se => se.Timestamp)
            .HasColumnName("timestamp")
            .IsRequired();

        builder.HasIndex(se => new { se.AggregateId, se.Version });
    }
}

