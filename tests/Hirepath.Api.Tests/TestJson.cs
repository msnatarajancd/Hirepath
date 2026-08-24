using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hirepath.Api.Tests;

/// <summary>JSON options mirroring the API: web defaults plus string enums.</summary>
public static class TestJson
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };
}
