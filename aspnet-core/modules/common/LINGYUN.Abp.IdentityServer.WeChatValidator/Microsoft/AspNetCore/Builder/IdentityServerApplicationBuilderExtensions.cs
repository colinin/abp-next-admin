using LINGYUN.Abp.IdentityServer;

namespace Microsoft.AspNetCore.Builder
{
    public static class IdentityServerApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWeChatSignature(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<WeChatSignatureMiddleware>();
            return builder;
        }
    }
}
