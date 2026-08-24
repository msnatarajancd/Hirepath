using Hirepath.Application.Common;

namespace Hirepath.Api.Endpoints;

/// <summary>Maps an application <see cref="Result{T}"/> onto an HTTP response.</summary>
public static class ResultExtensions
{
    public static IResult ToHttp<T>(this Result<T> result) =>
        result.IsSuccess
            ? Results.Ok(result.Value)
            : ToProblem(result.Error!);

    public static IResult ToHttp<T>(this Result<T> result, Func<T, IResult> onSuccess) =>
        result.IsSuccess ? onSuccess(result.Value!) : ToProblem(result.Error!);

    private static IResult ToProblem(AppError error)
    {
        var status = error.Code switch
        {
            AppErrorCode.NotFound => StatusCodes.Status404NotFound,
            AppErrorCode.Validation => StatusCodes.Status400BadRequest,
            AppErrorCode.Unsupported => StatusCodes.Status415UnsupportedMediaType,
            AppErrorCode.Unauthorized => StatusCodes.Status401Unauthorized,
            AppErrorCode.RateLimited => StatusCodes.Status429TooManyRequests,
            AppErrorCode.Timeout => StatusCodes.Status504GatewayTimeout,
            AppErrorCode.ProviderDown => StatusCodes.Status502BadGateway,
            _ => StatusCodes.Status400BadRequest,
        };

        return Results.Problem(
            title: error.Code.ToString(),
            detail: error.Message,
            statusCode: status);
    }
}
