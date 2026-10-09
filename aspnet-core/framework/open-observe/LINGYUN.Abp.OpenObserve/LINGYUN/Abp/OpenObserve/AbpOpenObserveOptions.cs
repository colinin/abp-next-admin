using LINGYUN.Abp.OpenObserve.Models;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace LINGYUN.Abp.OpenObserve;

public class AbpOpenObserveOptions
{
    /// <summary>
    /// 服务地址
    /// </summary>
    /// <remarks>
    /// 默认: http://localhost:5080
    /// </remarks>
    public string Endpoint { get; set; }
    /// <summary>
    /// 默认组织名称
    /// </summary>
    /// <remarks>
    /// 默认: default, 调用接口时未显式指定组织时使用
    /// </remarks>
    public string Organization { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string? Password { get; set; }
    /// <summary>
    /// 完整的授权头信息
    /// </summary>
    /// <remarks>
    /// 如: "Basic YWRtaW5AYWJwLmlvOnd3MVo1TCU2"
    /// 配置后优先于 UserName/Password
    /// </remarks>
    public string? AccessToken { get; set; }
    /// <summary>
    /// 请求超时时间(秒)
    /// </summary>
    /// <remarks>
    /// 默认: 600
    /// </remarks>
    public int TimeoutSeconds { get; set; }

    public AbpOpenObserveOptions()
    {
        Endpoint = OpenObserveConstants.DefaultEndpoint;
        Organization = OpenObserveConstants.DefaultOrganization;
        TimeoutSeconds = OpenObserveConstants.DefaultTimeoutSeconds;
    }

    /// <summary>
    /// 创建授权头
    /// </summary>
    /// <returns>未配置凭据时返回 null</returns>
    public virtual AuthenticationHeaderValue? CreateAuthorizationHeader()
    {
        if (!AccessToken.IsNullOrWhiteSpace() &&
            AuthenticationHeaderValue.TryParse(AccessToken, out var authenticationHeader))
        {
            return authenticationHeader;
        }

        if (!UserName.IsNullOrWhiteSpace() && !Password.IsNullOrWhiteSpace())
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{UserName}:{Password}"));

            return new AuthenticationHeaderValue("Basic", token);
        }

        return null;
    }
}
