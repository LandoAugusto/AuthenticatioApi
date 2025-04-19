using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Product.Api.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerJsonIgnoreFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters()
                .SelectMany(p => p.ParameterType.GetProperties()
                .Where(prop => CustomAttributeExtensions.GetCustomAttribute<JsonIgnoreAttribute>((MemberInfo)prop) != null))
                .ToList();

            if (!ignoredProperties.Any()) return;

            foreach (var property in ignoredProperties)
            {
                operation.Parameters = operation.Parameters
                    .Where(p => (!p.Name.Equals(property.Name, StringComparison.InvariantCulture)))
                    .ToList();
            }
        }
    }
}

