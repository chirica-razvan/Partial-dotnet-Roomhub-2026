using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid RoomId,
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees) : ICommand<CreateBookingResult>;

public sealed record CreateBookingResult(Guid Id, Guid RoomId, BookingStatus Status);

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider time)
    : ICommandHandler<CreateBookingCommand, CreateBookingResult>
{
    public async Task<Result<CreateBookingResult>> Handle(
        CreateBookingCommand command, CancellationToken cancellationToken)
    {
        // 404 before any validation (contract)
        var room = await rooms.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
            return RoomErrors.NotFound;

        var result = Booking.Create(
            room.Id,
            room.Capacity,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            time.GetUtcNow().UtcDateTime);

        if (result.IsFailure)
            return result.Error;

        var booking = result.Value;

        // R7: checks that need the database live in the handler; the overlap rule is in Booking.Overlaps
        var roomBookings = await bookings.ListActiveForRoomAsync(room.Id, cancellationToken);
        if (roomBookings.Any(booking.Overlaps))
            return BookingErrors.RoomOccupied;

        // R9
        var sameDay = await bookings.CountActiveForPersonOnDayAsync(booking.BookedBy, booking.StartUtc, cancellationToken);
        if (sameDay >= 2)
            return BookingErrors.DailyLimitReached;

        bookings.Add(booking);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateBookingResult(booking.Id, booking.RoomId, booking.Status);
    }
}
