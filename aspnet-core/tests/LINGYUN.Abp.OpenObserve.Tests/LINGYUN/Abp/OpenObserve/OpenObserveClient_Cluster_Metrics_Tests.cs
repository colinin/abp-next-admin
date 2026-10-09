using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.OpenObserve;

public class OpenObserveClient_Cluster_Metrics_Tests : AbpOpenObserveTestBase
{
    [Fact]
    public async Task Should_Get_Cluster_Info_With_Meta_Organization_By_Default()
    {
        HttpMessageHandler.EnqueueOk("{\"regions\":{\"openobserve\":{\"zo1\":{\"pending_jobs\":3}}}}");
        var client = CreateClient();

        var result = await client.GetClusterInfoAsync();

        // 实测集群信息仅对 _meta 组织开放
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/_meta/cluster/info");
        result.Regions.ContainsKey("openobserve").ShouldBeTrue();
        result.Regions["openobserve"]["zo1"].PendingJobs.ShouldBe(3);
    }

    [Fact]
    public async Task Should_Get_Metrics_As_Plain_Text()
    {
        HttpMessageHandler.EnqueueOk("# HELP process_cpu_seconds_total Total user and system CPU time spent in seconds.");
        var client = CreateClient();

        var result = await client.GetMetricsAsync();

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Get);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/metrics");
        result.ShouldStartWith("# HELP process_cpu_seconds_total");
    }

    [Fact]
    public async Task Should_Use_Configured_Organization_When_Not_Specified()
    {
        HttpMessageHandler.EnqueueOk("{\"list\":[]}");
        var client = CreateClient(options => options.Organization = "my-org");

        await client.GetStreamsAsync();

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/my-org/streams");
    }

    [Fact]
    public async Task Should_Use_Explicit_Organization_Over_Configured_One()
    {
        HttpMessageHandler.EnqueueOk("{\"list\":[]}");
        var client = CreateClient(options => options.Organization = "my-org");

        await client.GetStreamsAsync(organization: "_meta");

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/_meta/streams");
    }
}
