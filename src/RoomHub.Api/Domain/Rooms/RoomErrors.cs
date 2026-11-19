using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Rooms;

public static class RoomErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "Room.NotFound", "Sala nu a fost găsită.");

    public static readonly Error NameRequired = Error.Validation(
        "Room.NameRequired", "Numele sălii este obligatoriu.");

    public static readonly Error InvalidCapacity = Error.Validation(
        "Room.InvalidCapacity", "Capacitatea trebuie să fie strict pozitivă.");
}
