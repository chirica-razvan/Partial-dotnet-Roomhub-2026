using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Common;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/{id:guid}/cancel", Handle)
            .WithName("CancelBooking")
            .WithSummary("Anulează o rezervare")
            .WithDescription("Active -> Cancelled. 404 dacă nu există, 409 dacă e deja anulată (R8) "
                + "sau începe în mai puțin de 60 de minute (R11).")
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

    private static async Task<Results<NoContent, ProblemHttpResult>> Handle(
        Guid id,
        ICommandHandler<CancelBookingCommand, Unit> handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new CancelBookingCommand(id), ct);
        if (result.IsFailure)
            return result.Error.ToProblem();

        return TypedResults.NoContent();
    }
}
