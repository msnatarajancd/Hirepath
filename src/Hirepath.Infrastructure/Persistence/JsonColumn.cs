using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hirepath.Infrastructure.Persistence;

/// <summary>
/// Helpers to store a complex property as a JSON text column in SQLite, with a
/// matching value comparer so EF change-tracking works on the deserialised graph.
/// Deliberately boring: one place that knows how our JSON columns behave.
/// </summary>
public static class JsonColumn
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    public static ValueConverter<T, string> Converter<T>() => new(
        value => JsonSerializer.Serialize(value, Options),
        text => Deserialize<T>(text));

    public static ValueComparer<T> Comparer<T>() => new(
        (a, b) => JsonSerializer.Serialize(a, Options) == JsonSerializer.Serialize(b, Options),
        v => v == null ? 0 : JsonSerializer.Serialize(v, Options).GetHashCode(),
        v => Deserialize<T>(JsonSerializer.Serialize(v, Options)));

    private static T Deserialize<T>(string text) =>
        string.IsNullOrWhiteSpace(text)
            ? default!
            : JsonSerializer.Deserialize<T>(text, Options)!;
}
