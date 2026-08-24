using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Hirepath.Api.OpenApi;

/// <summary>
/// Represents <see cref="IFormFile"/> as a binary string in the OpenAPI
/// document so NSwag generates a real file parameter for uploads rather than
/// dropping it as an unrecognised object.
/// </summary>
public sealed class FormFileSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type == typeof(IFormFile))
        {
            schema.Type = JsonSchemaType.String;
            schema.Format = "binary";
            schema.Properties?.Clear();
            schema.AllOf?.Clear();
        }

        return Task.CompletedTask;
    }
}
