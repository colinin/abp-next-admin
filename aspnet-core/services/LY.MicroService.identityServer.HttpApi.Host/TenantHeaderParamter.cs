using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace LY.MicroService.IdentityServer;

public class TenantHeaderParamter : IOperationFilter
{
    private readonly AbpMultiTenancyOptions _options;
    public TenantHeaderParamter(
        IOptions<AbpMultiTenancyOptions> options)
    {
        _options = options.Value;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (_options.IsEnabled)
        {
            operation.Parameters = operation.Parameters ?? new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = TenantResolverConsts.DefaultTenantKey,
                In = ParameterLocation.Header,
                Description = "Tenant Id in http header",
                Required = false
            });
        }
    }
}
