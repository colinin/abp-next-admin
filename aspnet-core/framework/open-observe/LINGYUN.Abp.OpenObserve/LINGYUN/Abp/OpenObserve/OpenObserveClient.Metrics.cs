using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 内部指标(Prometheus 文本格式)
    /// </summary>
    /// <remarks>
    /// GET /metrics, 注意该接口不在 /api 前缀下
    /// 需要服务端开启 ZO_PROMETHEUS_ENABLE=true, 未开启时返回空内容
    /// 参考: https://openobserve.ai/docs/reference/api/metrics/
    /// </remarks>
    /// <returns>Prometheus 文本格式内容(text/plain; version=0.0.4)</returns>
    public async virtual Task<string> GetMetricsAsync(
        CancellationToken cancellationToken = default)
    {
        var httpResponse = await HttpClient.GetAsync("/metrics", cancellationToken);

        await EnsureSuccessStatusCode(httpResponse, null, cancellationToken);

        return await httpResponse.Content.ReadAsStringAsync(cancellationToken);
    }
}
