using System;

namespace LINGYUN.Abp.OpenApi;

public class AbpOpenApiOptions
{
    /// <summary>
    /// 启用Api签名检查
    /// </summary>
    /// <remarks>
    /// 默认: true
    /// </remarks>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 请求随机数过期时间
    /// </summary>
    /// <remarks>
    /// 默认: 10分钟
    /// </remarks>
    public TimeSpan RequestNonceExpireIn { get; set; }

    public AbpOpenApiOptions()
    {
        IsEnabled = true;

        RequestNonceExpireIn = TimeSpan.FromMinutes(10);
    }
}
