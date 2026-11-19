using RoomHub.Api.Common;
using RoomHub.Api.Infrastructure.Persistence.Repositories;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public static class GetRoomEndpoint
{
    public sealed record Response(Guid Id, string Name, int Capacity);

    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/{id:guid}", Handle).WithName("GetRoom");

    private static IResult Handle(Guid id, RoomRepository rooms)
    {
        var room = rooms.Get(id)
            ?? throw new BusinessException("RoomNotFound", "The room does not exist.", 404);

        return Results.Ok(new Response(room.Id, room.Name, room.Capacity));
    }
}
