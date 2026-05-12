using Microsoft.OpenApi;
using System.ComponentModel;
using System.Reflection;

namespace Swashbuckle.AspNetCore.SwaggerGen;

#nullable enable
public class EnumDescriptionSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var enumType = context.Type;
            var enumValues = Enum.GetValues(enumType);
            var underlyingType = Enum.GetUnderlyingType(enumType);

            var enumDesc = enumType.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumType.Name;
            schema.Description += $"{enumDesc}<br />";

            schema.Description += "<p>Options:</p><ul>";

            foreach (var value in enumValues)
            {
                string? description = default!;
                var name = Enum.GetName(enumType, value);
                if (name != null)
                {
                    var memberInfo = enumType.GetMember(name)[0];
                    description = memberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ?? name;
                }
                var numericValue = Convert.ChangeType(value, underlyingType);
                schema.Description += $"<li><b>{numericValue}</b>: {name}({description ?? "No description."})</li>";
            }

            schema.Description += "</ul>";
        }
    }
}
#nullable disable
