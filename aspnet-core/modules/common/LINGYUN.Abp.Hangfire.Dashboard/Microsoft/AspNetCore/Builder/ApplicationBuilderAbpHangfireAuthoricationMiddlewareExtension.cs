using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderAbpHangfireAuthoricationMiddlewareExtension
    {
        public static IApplicationBuilder UseHangfireAuthorication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HangfireAuthoricationMiddleware>();
        }
    }
}
