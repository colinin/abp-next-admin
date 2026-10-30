using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MicroService.WorkflowService.Authentication;

/// <summary>
/// 让 elsa/api 走 JWT（Bearer）认证的认证方案提供程序。
/// </summary>
/// <remarks>
/// 本服务为了支持 Elsa Studio（Blazor Server）的登录跳转，把默认方案设成了 Cookie、
/// 默认质询方案设成了 OpenIdConnect。而授权管道（PolicyEvaluator）在策略没有显式指定方案时，
/// 会用默认方案重新认证并覆盖 HttpContext.User，于是携带 Bearer 的请求会被换成匿名 Cookie 主体，
/// 授权失败后按默认质询方案 302 跳转到登录页。
/// 这里按请求特征改写默认方案：
/// <list type="bullet">
/// <item>elsa/api 且带 Bearer 头：认证用 JwtBearer（AuthServer/OpenIddict 颁发的 access_token 也是 JWT）。</item>
/// <item>elsa/api 未带 Bearer 头：认证沿用默认方案（Cookie），供 Studio 服务端调用使用。</item>
/// <item>elsa/api 认证/授权失败：质询用 JwtBearer，返回 401 而不是跳转登录页。</item>
/// <item>其它路径：完全保持原有行为（Cookie 认证 + OIDC 登录跳转）。</item>
/// </list>
/// </remarks>
public sealed class ElsaApiAuthenticationSchemeProvider(
    IOptions<AuthenticationOptions> options,
    IHttpContextAccessor httpContextAccessor) : AuthenticationSchemeProvider(options)
{
    private const string ElsaApiPathPrefix = "/elsa/api";
    private const string BearerPrefix = "Bearer ";

    /// <inheritdoc />
    public override async Task<AuthenticationScheme> GetDefaultAuthenticateSchemeAsync()
    {
        if (IsElsaApiRequest(out var hasBearerToken) && hasBearerToken)
        {
            return await GetSchemeAsync(JwtBearerDefaults.AuthenticationScheme)
                   ?? await base.GetDefaultAuthenticateSchemeAsync();
        }

        return await base.GetDefaultAuthenticateSchemeAsync();
    }

    /// <inheritdoc />
    public override async Task<AuthenticationScheme> GetDefaultChallengeSchemeAsync()
    {
        if (IsElsaApiRequest(out _))
        {
            return await GetSchemeAsync(JwtBearerDefaults.AuthenticationScheme)
                   ?? await base.GetDefaultChallengeSchemeAsync();
        }

        return await base.GetDefaultChallengeSchemeAsync();
    }

    private bool IsElsaApiRequest(out bool hasBearerToken)
    {
        hasBearerToken = false;

        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return false;
        }

        hasBearerToken = httpContext.Request.Headers.Authorization
            .ToString()
            .StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase);

        return httpContext.Request.Path.StartsWithSegments(ElsaApiPathPrefix, StringComparison.OrdinalIgnoreCase);
    }
}
