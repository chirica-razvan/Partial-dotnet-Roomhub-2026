using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Rooms;

// Rich domain: no public constructor, no public setters. The only entry point is Room.Create.
public sealed class Room
{
    private Room()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public int Capacity { get; private set; }

    public static Result<Room> Create(string? name, int capacity, Guid? id = null)
    {
        // R1
        if (string.IsNullOrWhiteSpace(name))
            return RoomErrors.NameRequired;

        if (capacity <= 0)
            return RoomErrors.InvalidCapacity;

        return new Room
        {
            Id = id ?? Guid.NewGuid(),
            Name = name.Trim(),
            Capacity = capacity
        };
    }
}
