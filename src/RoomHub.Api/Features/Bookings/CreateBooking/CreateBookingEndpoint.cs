using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Common;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public sealed record Request(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

    public sealed record Response(Guid Id, Guid RoomId, BookingStatus Status);

    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/", Handle)
            .WithName("CreateBooking")
            .WithSummary("Creează o rezervare pentru o sală")
            .WithDescription("404 dacă sala nu există, 400 dacă datele nu sunt valide (R2–R6, R10), "
                + "409 dacă sala e ocupată (R7) sau persoana are deja 2 rezervări active în acea zi (R9).")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

    private static async Task<Results<Created<Response>, ProblemHttpResult>> Handle(
        Guid roomId,
        Request request,
        ICommandHandler<CreateBookingCommand, CreateBookingResult> handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(
            new CreateBookingCommand(roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees),
            ct);

        if (result.IsFailure)
            return result.Error.ToProblem();

        var booking = result.Value;
        return TypedResults.Created(
            $"/api/bookings/{booking.Id}",
            new Response(booking.Id, booking.RoomId, booking.Status));
    }
}
