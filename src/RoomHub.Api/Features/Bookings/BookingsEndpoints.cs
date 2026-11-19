using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;

namespace RoomHub.Api.Features.Bookings;

public static class BookingsEndpoints
{
    public static void MapBookings(this IEndpointRouteBuilder app)
    {
        var roomBookings = app.MapGroup("/api/rooms/{roomId:guid}/bookings").WithTags("Bookings");
        CreateBookingEndpoint.Map(roomBookings);

        var group = app.MapGroup("/api/bookings").WithTags("Bookings");
        GetBookingEndpoint.Map(group);
        CancelBookingEndpoint.Map(group);
    }
}
