using LINGYUN.Abp.AspNetCore.Wrapper;

namespace Microsoft.AspNetCore.Builder;
public static class AbpWrapperApplicationBuilderExtensions
{
    private const string ExceptionHandlingMiddlewareMarker = "_AbpExceptionHandlingMiddleware_Added";

    public static IApplicationBuilder UseWrapperExceptionHandling(this IApplicationBuilder app)
    {
        if (app.Properties.ContainsKey(ExceptionHandlingMiddlewareMarker))
        {
            return app;
        }

        app.Properties[ExceptionHandlingMiddlewareMarker] = true;
        return app.UseMiddleware<AbpExceptionHandlingWrapperMiddleware>();
    }
}
