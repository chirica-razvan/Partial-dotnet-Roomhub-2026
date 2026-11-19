using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

// shortcut: in-memory data, lost on restart; replaced by EF Core + SQLite at level C.
public sealed class RoomRepository : IRoomRepository
{
    private readonly List<Room> _rooms = [];
    private readonly Lock _lock = new();

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        lock (_lock)
            return Task.FromResult(_rooms.FirstOrDefault(r => r.Id == id));
    }

    public void Add(Room room)
    {
        lock (_lock)
            _rooms.Add(room);
    }
}
