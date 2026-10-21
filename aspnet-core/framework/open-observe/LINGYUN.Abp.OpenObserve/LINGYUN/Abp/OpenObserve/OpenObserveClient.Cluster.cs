using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.Cluster;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 集群信息
    /// </summary>
    /// <remarks>
    /// 返回各区域、各节点的待处理压缩任务数
    /// 实测该接口仅对 _meta 组织开放, 使用其它组织返回 403 "This API is only available for the _meta organization",
    /// 因此 organization 默认取 _meta
    /// 文档给出的路径为 GET /api/{org_id}/cluster_info, 实测为 404, 实际路径为 GET /api/{org_id}/cluster/info
    /// 参考: https://openobserve.ai/docs/reference/api/cluster/cluster-info/
    /// </remarks>
    public async virtual Task<ClusterInfoResponse> GetClusterInfoAsync(
        string? organization = OpenObserveConstants.MetaOrganization,
        CancellationToken cancellationToken = default)
    {
        var requestUri = CreateClusterInfoRequestUri(ResolveOrganization(organization));

        return (await GetAsync<ClusterInfoResponse>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 创建集群信息请求地址
    /// </summary>
    /// <remarks>
    /// 文档与实现的路径不一致, 需要兼容时重写该方法
    /// </remarks>
    protected virtual string CreateClusterInfoRequestUri(string organization)
    {
        return $"/api/{organization}/cluster/info";
    }
}
