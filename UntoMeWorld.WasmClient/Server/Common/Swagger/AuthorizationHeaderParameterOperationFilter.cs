using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UntoMeWorld.WasmClient.Server.Common.Swagger;

public class AuthorizationHeaderParameterOperationFilter: IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(d => d is AuthorizeAttribute))
            return;
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter 
        {
            Name = "ApiToken",
            In = ParameterLocation.Header,
            Description = "access token",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString(""),
            }
        });
    }
}