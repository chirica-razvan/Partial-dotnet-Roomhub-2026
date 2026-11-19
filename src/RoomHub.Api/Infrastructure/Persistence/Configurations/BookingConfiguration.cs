using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(b => b.Id);

        // The Id is generated in the domain, not by the database.
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.Property(b => b.BookedBy).IsRequired();
        builder.Property(b => b.Attendees).IsRequired();

        builder.Property(b => b.StartUtc).IsRequired().HasConversion<UtcDateTimeConverter>();
        builder.Property(b => b.EndUtc).IsRequired().HasConversion<UtcDateTimeConverter>();

        // Enums are stored as text.
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);

        // A booking belongs to an existing room; no navigation property in the domain.
        builder.HasOne<Room>()
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
