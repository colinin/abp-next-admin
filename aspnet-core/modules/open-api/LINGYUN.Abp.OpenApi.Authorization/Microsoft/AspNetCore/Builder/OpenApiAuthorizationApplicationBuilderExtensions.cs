using LINGYUN.Abp.OpenApi.Authorization;

namespace Microsoft.AspNetCore.Builder
{
    public static class OpenApiAuthorizationApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseOpenApiAuthorization(this IApplicationBuilder app)
        {
            return app.UseMiddleware<OpenApiAuthorizationMiddleware>();
        }
    }
}
