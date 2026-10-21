using LINGYUN.Abp.OpenObserve.Models.Search;
using LINGYUN.Abp.OpenObserve.Models.Stream;
using Shouldly;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.OpenObserve;

public class OpenObserveClient_Search_Tests : AbpOpenObserveTestBase
{
    private static long ToMicroseconds(DateTime time)
    {
        return (time.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks) / 10;
    }

    [Fact]
    public async Task Should_Search_With_Query_Body()
    {
        HttpMessageHandler.EnqueueOk("{\"took\":12,\"total\":1,\"hits\":[{\"_timestamp\":1}],\"from\":0,\"size\":10,\"scan_size\":3}");
        var client = CreateClient();

        var startTime = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var endTime = new DateTime(2024, 1, 2, 4, 4, 5, DateTimeKind.Utc);

        var result = await client.SearchAsync<JsonNode>(
            new SearchQuery("SELECT * FROM my-stream", startTime, endTime, from: 0, size: 10),
            SearchType.Dashboards,
            timeout: 300,
            useCache: false);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Post);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/_search?use_cache=false");

        var body = HttpMessageHandler.LastRequest.Body!;
        body.ShouldContain("\"sql\":\"SELECT * FROM my-stream\"");
        body.ShouldContain($"\"start_time\":{ToMicroseconds(startTime)}");
        body.ShouldContain($"\"end_time\":{ToMicroseconds(endTime)}");
        body.ShouldContain("\"size\":10");
        body.ShouldContain("\"search_type\":\"dashboards\"");
        body.ShouldContain("\"timeout\":300");

        result.Took.ShouldBe(12);
        result.Hits.Length.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Search_With_Agent_Options()
    {
        HttpMessageHandler.EnqueueOk("{\"took\":1,\"hits\":[],\"format\":\"csv\",\"data\":\"x,y\"}");
        var client = CreateClient();

        var result = await client.SearchAsync<JsonNode>(
            new SearchQuery("SELECT * FROM my-stream", DateTime.UtcNow.AddHours(-1), DateTime.UtcNow),
            agentOptions: new SearchAgentOptions(SearchAgentMode.Partition, SearchAgentOutputFormat.Csv));

        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"agent_options\":{\"mode\":\"partition\",\"output_format\":\"csv\"}");
        result.Format.ShouldBe("csv");
        result.Data.ShouldBe("x,y");
    }

    [Fact]
    public async Task Should_Throw_Search_Exception_With_Error_Detail()
    {
        HttpMessageHandler.Enqueue(
            HttpStatusCode.BadRequest,
            "{\"code\":20004,\"message\":\"unknown field 'servce'\",\"suggestions\":[\"service\"]}");
        var client = CreateClient();

        var exception = await Should.ThrowAsync<OpenObserveSearchException>(async () =>
            await client.SearchAsync<JsonNode>(
                new SearchQuery("SELECT servce FROM my-stream", DateTime.UtcNow.AddHours(-1), DateTime.UtcNow)));

        exception.Code.ShouldBe(20004);
        exception.StatusCode.ShouldBe(400);
        exception.Error.ShouldNotBeNull();
        exception.Error!.Suggestions.ShouldBe(["service"]);
    }

    [Fact]
    public async Task Should_Get_Values_With_Query_Parameters()
    {
        HttpMessageHandler.EnqueueOk("{\"took\":5,\"hits\":[{\"field\":\"level\",\"values\":[{\"zo_sql_key\":\"info\",\"zo_sql_num\":2}]}],\"total\":1}");
        var client = CreateClient();

        var startTime = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var endTime = new DateTime(2024, 1, 2, 4, 4, 5, DateTimeKind.Utc);

        var result = await client.GetValuesAsync(
            "my-stream",
            ["level", "service"],
            startTime,
            endTime,
            size: 10,
            keyword: "in",
            noCount: true);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Get);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe(
            $"/api/default/my-stream/_values?fields=level%2Cservice&start_time={ToMicroseconds(startTime)}&end_time={ToMicroseconds(endTime)}&size=10&keyword=in&no_count=true");
        result.Hits.Length.ShouldBe(1);
        result.Hits[0].Values[0].Key.ShouldBe("info");
    }

    [Fact]
    public async Task Should_Get_Around_With_Query_Parameters()
    {
        HttpMessageHandler.EnqueueOk("{\"took\":32,\"hits\":[{\"_timestamp\":1}],\"from\":0,\"size\":2}");
        var client = CreateClient();

        var result = await client.AroundAsync<JsonNode>("my-stream", 1700000000000000L, size: 2, type: StreamType.Logs);

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/my-stream/_around?key=1700000000000000&size=2&type=logs");
        result.Size.ShouldBe(2);
        result.Hits.Length.ShouldBe(1);
    }
}
