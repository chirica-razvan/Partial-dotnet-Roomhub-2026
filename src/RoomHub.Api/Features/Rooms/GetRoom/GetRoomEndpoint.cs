using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.GetRoom;

// Read: no command handler. At level C it moves to DbContext + AsNoTracking.
public static class GetRoomEndpoint
{
    public sealed record Response(Guid Id, string Name, int Capacity);

    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetRoom")
            .WithSummary("Returnează o sală după id")
            .WithDescription("404 dacă sala nu există.")
            .ProducesProblem(StatusCodes.Status404NotFound);

    private static async Task<Results<Ok<Response>, ProblemHttpResult>> Handle(
        Guid id,
        IRoomRepository rooms,
        CancellationToken ct)
    {
        var room = await rooms.GetByIdAsync(id, ct);
        if (room is null)
            return RoomErrors.NotFound.ToProblem();

        return TypedResults.Ok(new Response(room.Id, room.Name, room.Capacity));
    }
}
