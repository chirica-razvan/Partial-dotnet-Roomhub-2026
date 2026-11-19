using RoomHub.Api.Domain.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace RoomHub.Api.Common;

// The only place where an Error becomes an HTTP response (ProblemDetails, RFC 9457).
public static class ErrorHttpExtensions
{
    public static ProblemHttpResult ToProblem(this Error error)
    {
        var status = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return TypedResults.Problem(
            detail: error.Message,
            statusCode: status,
            title: error.Code,
            extensions: new Dictionary<string, object?> { ["code"] = error.Code });
    }
}
