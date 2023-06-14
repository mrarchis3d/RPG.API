using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RPG.BuildingBlocks.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class RemoveInternalEndpoints : IDocumentFilter
{
    private static List<string> _policiesToRemove = new List<string>()
    {
        Authorization.SERVICE_DISCOVERY,
        Authorization.EVENTBUS
    };
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var apiDescription in context.ApiDescriptions)
        {
            MethodInfo methodInfo;
            var success = apiDescription.TryGetMethodInfo(out methodInfo);
            if (success)
            {
                var controllerAuth = methodInfo.DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Select(attr => attr.Policy)
                    .Distinct();
                
                var methodAuth = methodInfo
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Select(attr => attr.Policy)
                    .Distinct();

                if (!controllerAuth.Any(schema => _policiesToRemove.Contains(schema)) &&
                    !methodAuth.Any(schema => _policiesToRemove.Contains(schema))) continue;
                
                if (apiDescription.RelativePath == null) continue;
                    
                var route = "/" + apiDescription.RelativePath.TrimEnd('/');
                swaggerDoc.Paths.Remove(route);
            }

        }
    }
}