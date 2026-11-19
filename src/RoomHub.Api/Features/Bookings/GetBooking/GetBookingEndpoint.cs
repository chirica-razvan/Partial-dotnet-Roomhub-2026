using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Common;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Features.Bookings.GetBooking;

// Read: no command handler. At level C it moves to DbContext + AsNoTracking.
public static class GetBookingEndpoint
{
    public sealed record Response(
        Guid Id,
        Guid RoomId,
        string BookedBy,
        DateTime StartUtc,
        DateTime EndUtc,
        int Attendees,
        BookingStatus Status);

    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetBooking")
            .WithSummary("Returnează o rezervare după id")
            .WithDescription("404 dacă rezervarea nu există.")
            .ProducesProblem(StatusCodes.Status404NotFound);

    private static async Task<Results<Ok<Response>, ProblemHttpResult>> Handle(
        Guid id,
        IBookingRepository bookings,
        CancellationToken ct)
    {
        var b = await bookings.GetByIdAsync(id, ct);
        if (b is null)
            return BookingErrors.NotFound.ToProblem();

        return TypedResults.Ok(new Response(
            b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status));
    }
}
