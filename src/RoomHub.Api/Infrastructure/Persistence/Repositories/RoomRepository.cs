using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class RoomRepository(RoomHubDbContext db) : IRoomRepository
{
    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await db.Rooms.FindAsync([id], cancellationToken);

    // Only marks the room as Added; IUnitOfWork.SaveChangesAsync writes it.
    public void Add(Room room) => db.Rooms.Add(room);
}
