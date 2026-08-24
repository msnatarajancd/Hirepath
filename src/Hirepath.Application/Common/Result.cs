namespace Hirepath.Application.Common;

/// <summary>
/// A lightweight result carrying either a value or a typed error. Keeps
/// use-case handlers from throwing for expected, recoverable failures.
/// </summary>
public readonly record struct Result<T>
{
    internal Result(bool ok, T? value, AppError? error)
    {
        IsSuccess = ok;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public T? Value { get; }
    public AppError? Error { get; }

    public static implicit operator Result<T>(T value) => new(true, value, null);
    public static implicit operator Result<T>(AppError error) => new(false, default, error);
}

/// <summary>Non-generic factory helpers, so the generic type carries no static
/// members (CA1000).</summary>
public static class Result
{
    public static Result<T> Ok<T>(T value) => new(true, value, null);
    public static Result<T> Fail<T>(AppError error) => new(false, default, error);
}

/// <summary>A typed, user-facing error. Codes map to recoverable UI states.</summary>
public sealed record AppError(AppErrorCode Code, string Message)
{
    public static AppError NotFound(string what) =>
        new(AppErrorCode.NotFound, $"{what} was not found.");

    public static AppError Validation(string message) =>
        new(AppErrorCode.Validation, message);

    public static AppError Unsupported(string message) =>
        new(AppErrorCode.Unsupported, message);
}

public enum AppErrorCode
{
    Validation = 0,
    NotFound = 1,
    Unsupported = 2,
    // AI provider surface (used from Phase 2): RateLimited, Unauthorized,
    // Timeout, InvalidResponse, ProviderDown.
    RateLimited = 10,
    Unauthorized = 11,
    Timeout = 12,
    InvalidResponse = 13,
    ProviderDown = 14,
}
