/*
A helper class to transform openApi documentation and make it more readable.
*/

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

public class OpenApiVersionTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        var version = context.DocumentName.Replace("v", "");

        document.Info.Title = $"Music Database API v{version}";
        document.Info.Version = version;
        document.Info.Description = $"Version {version} of the Music Database API";

        return Task.CompletedTask;
    }
}
