namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken);

    // R9: a person's active bookings on the given UTC day, across all rooms.
    Task<int> CountActiveForPersonOnDayAsync(string bookedBy, DateTime dayUtc, CancellationToken cancellationToken);

    void Add(Booking booking);
}
