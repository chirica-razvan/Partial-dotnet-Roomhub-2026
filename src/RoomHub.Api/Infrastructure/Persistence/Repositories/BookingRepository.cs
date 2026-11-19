using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

// shortcut: in-memory data, lost on restart; replaced by EF Core + SQLite at level C.
public sealed class BookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = [];
    private readonly Lock _lock = new();

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        lock (_lock)
            return Task.FromResult(_bookings.FirstOrDefault(b => b.Id == id));
    }

    public Task<IReadOnlyList<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            IReadOnlyList<Booking> result = _bookings
                .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
                .ToList();
            return Task.FromResult(result);
        }
    }

    public Task<int> CountActiveForPersonOnDayAsync(string bookedBy, DateTime dayUtc, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            return Task.FromResult(_bookings.Count(b =>
                b.Status == BookingStatus.Active
                && b.BookedBy == bookedBy
                && b.StartUtc.Date == dayUtc.Date));
        }
    }

    public void Add(Booking booking)
    {
        lock (_lock)
            _bookings.Add(booking);
    }
}
