namespace RoomHub.Api.Features.Rooms.CreateRoom;

public static class CreateRoomEndpoint
{
    public sealed record Request(string? Name, int Capacity);

    public sealed record Response(Guid Id, string Name, int Capacity);

    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/", Handle).WithName("CreateRoom");

    private static IResult Handle(Request request, CreateRoomHandler handler)
    {
        var room = handler.Handle(request.Name, request.Capacity);
        return Results.Created($"/api/rooms/{room.Id}", new Response(room.Id, room.Name, room.Capacity));
    }
}
