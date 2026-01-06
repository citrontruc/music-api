/*
A helper class to transform openApi documentation and make it more readable.
*/

using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

public sealed class ConfigureOpenApiOptions : IOpenApiDocumentTransformer
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureOpenApiOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        var description = _provider.ApiVersionDescriptions.Single(d =>
            d.GroupName == context.DocumentName
        );

        document.Info ??= new OpenApiInfo();
        document.Info.Title = $"Music Database API v{description.ApiVersion}";
        document.Info.Version = description.ApiVersion.ToString();
        document.Info.Description = $"Version {description.ApiVersion} of the Music Database API";

        return Task.CompletedTask;
    }
}
