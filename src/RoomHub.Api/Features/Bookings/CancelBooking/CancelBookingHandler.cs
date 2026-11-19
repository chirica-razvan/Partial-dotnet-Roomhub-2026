using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed record CancelBookingCommand(Guid BookingId) : ICommand<Unit>;

public sealed class CancelBookingHandler(IBookingRepository bookings, TimeProvider time)
    : ICommandHandler<CancelBookingCommand, Unit>
{
    public async Task<Result<Unit>> Handle(CancelBookingCommand command, CancellationToken cancellationToken)
    {
        var booking = await bookings.GetByIdAsync(command.BookingId, cancellationToken);
        if (booking is null)
            return BookingErrors.NotFound;

        // R8 + R11 live in Booking.Cancel; the handler never touches Status directly
        return booking.Cancel(time.GetUtcNow().UtcDateTime);
    }
}
