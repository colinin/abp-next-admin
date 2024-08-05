using Microsoft.AspNetCore.Builder;

namespace LINGYUN.Abp.Identity.Session.AspNetCore;
public static class AbpSessionApplicationBuilderExtensions
{
    /// <summary>
    /// 会话管理中间件
    /// </summary>
    /// <remarks>
    /// 建议顺序:<br />
    /// <seealso cref="AuthAppBuilderExtensions.UseAuthentication(IApplicationBuilder)"/><br />
    /// <seealso cref="AbpSessionApplicationBuilderExtensions.UseAbpSession(IApplicationBuilder)"/><br />
    /// <seealso cref="AbpApplicationBuilderExtensions.UseDynamicClaims(IApplicationBuilder)"/><br />
    /// <seealso cref="AuthorizationAppBuilderExtensions.UseAuthorization(IApplicationBuilder)"/><br />
    /// </remarks>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAbpSession(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AbpSessionMiddleware>();
    }
}
