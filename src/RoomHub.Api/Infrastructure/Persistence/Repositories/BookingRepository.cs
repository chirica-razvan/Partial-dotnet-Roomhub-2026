using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await db.Bookings.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken) =>
        await db.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);

    public Task<int> CountActiveForPersonOnDayAsync(string bookedBy, DateTime dayUtc, CancellationToken cancellationToken)
    {
        // A [day, next day) range instead of StartUtc.Date, so the filter stays plain SQL.
        var dayStart = DateTime.SpecifyKind(dayUtc.Date, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);

        return db.Bookings.CountAsync(
            b => b.Status == BookingStatus.Active
                && b.BookedBy == bookedBy
                && b.StartUtc >= dayStart
                && b.StartUtc < dayEnd,
            cancellationToken);
    }

    // Only marks the booking as Added; IUnitOfWork.SaveChangesAsync writes it.
    public void Add(Booking booking) => db.Bookings.Add(booking);
}
