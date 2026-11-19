namespace RoomHub.Api.Domain.Common;

// "No value" for Result<Unit>: operations that only succeed or fail.
public readonly record struct Unit
{
    public static readonly Unit Value = new();
}
