namespace RoomHub.Api.Domain.Common;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict
}

// Code is stable (clients rely on it); Message may be reworded.
public sealed record Error(string Code, string Message, ErrorType Type)
{
    public static Error Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);
}
