using System;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.Telemetry.OpenTelemetry;
public class AbpTelemetryOpenTelemetryOptions
{
    /// <summary>
    /// 是否忽略CAP仪表板请求, 默认: true
    /// </summary>
    public bool IgnoreCapDashboardUrls { get; set; }
    /// <summary>
    /// 是否忽略ES请求, 默认: true
    /// </summary>
    public bool IgnoreElasticsearchUrls { get; set; }
    /// <summary>
    /// 忽略本地请求路径
    /// </summary>
    public List<string> IgnoreLocalRequestUrls { get; set; }
    /// <summary>
    /// 忽略远程请求路径
    /// </summary>
    public List<string> IgnoreRemoteRequestUrls { get; set; }

    public AbpTelemetryOpenTelemetryOptions()
    {
        IgnoreLocalRequestUrls = new List<string>
        {
             "/healthz",    // 服务健康状态请求不记录
        };
        IgnoreRemoteRequestUrls = new List<string>();
        IgnoreCapDashboardUrls = true;
        IgnoreElasticsearchUrls = true;
    }

    public bool IsIgnureLocalRequestUrl(string url)
    {
        // 忽略CAP仪表板请求
        if (IgnoreCapDashboardUrls && url.StartsWith("/cap"))
        {
            return true;
        }

        return IgnoreLocalRequestUrls.Any(localUrl => url.Equals(localUrl, StringComparison.InvariantCultureIgnoreCase));
    }

    public bool IsIgnureRemoteRequestUrl(string url)
    {
        // 忽略向es推送数据请求
        if (IgnoreElasticsearchUrls && url.EndsWith("/_bulk"))
        {
            return true;
        }

        return IgnoreRemoteRequestUrls.Any(remoteUrl => url.Equals(remoteUrl, StringComparison.InvariantCultureIgnoreCase));
    }
}

