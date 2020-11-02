using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class SignalRJwtTokenApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSignalRJwtToken(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SignalRJwtTokenMiddleware>();
        }
    }
}
