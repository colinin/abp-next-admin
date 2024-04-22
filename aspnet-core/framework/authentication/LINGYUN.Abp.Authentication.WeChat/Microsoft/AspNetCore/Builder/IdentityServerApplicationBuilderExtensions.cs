using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class IdentityServerApplicationBuilderExtensions
    {
        /// <summary>
        /// 启用中间件可以处理微信服务器消息
        /// 用于验证消息是否来自于微信服务器
        /// </summary>
        /// <param name="builder"></param>
        /// <remarks>
        /// 也可以用Controller的形式来实现
        /// </remarks>
        /// <returns></returns>
        public static IApplicationBuilder UseWeChatSignature(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<WeChatOfficialSignatureMiddleware>();
            return builder;
        }
    }
}
