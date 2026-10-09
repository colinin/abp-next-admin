using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.Stream;
using Shouldly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.OpenObserve;

public class OpenObserveClient_Stream_Tests : AbpOpenObserveTestBase
{
    private static long ToMicroseconds(DateTime time)
    {
        return (time.ToUniversalTime().Ticks - DateTime.UnixEpoch.Ticks) / 10;
    }

    [Fact]
    public async Task Should_Get_Streams_With_Query_Parameters()
    {
        HttpMessageHandler.EnqueueOk("{\"list\":[{\"name\":\"my-stream\",\"storage_type\":\"disk\",\"stream_type\":\"logs\"}]}");
        var client = CreateClient();

        var result = await client.GetStreamsAsync(fetchSchema: true, type: StreamType.Metrics);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Get);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/streams?fetchSchema=true&type=metrics");
        result.List.Length.ShouldBe(1);
        result.List[0].Name.ShouldBe("my-stream");
    }

    [Fact]
    public async Task Should_Get_Stream_Schema()
    {
        HttpMessageHandler.EnqueueOk("{\"name\":\"my-stream\",\"schema\":[{\"name\":\"_timestamp\",\"type\":\"Int64\"}]}");
        var client = CreateClient();

        var result = await client.GetStreamSchemaAsync("my-stream", type: StreamType.Traces);

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/streams/my-stream/schema?type=traces");
        result.Schema.ShouldNotBeNull();
        result.Schema!.Length.ShouldBe(1);
        result.Schema[0].Name.ShouldBe("_timestamp");
    }

    [Fact]
    public async Task Should_Create_Stream_Settings()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200}");
        var client = CreateClient();

        var result = await client.CreateStreamSettingsAsync(
            "my-stream",
            partitionKeys: ["k8s_cluster"],
            dataRetention: 30,
            storeOriginalData: true,
            storageType: "compliance");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Post);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/streams/my-stream/settings");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"partition_keys\":[\"k8s_cluster\"]");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"data_retention\":30");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"store_original_data\":true");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"storage_type\":\"compliance\"");
        result.Code.ShouldBe(200);
    }

    [Fact]
    public async Task Should_Update_Stream_Settings_With_Wrapper()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200}");
        var client = CreateClient();

        await client.UpdateStreamSettingsAsync(
            "my-stream",
            partitionKeys: new UpdateStreamSettingValue<string> { Set = ["a", "b"], Add = ["c"] },
            fullTextSearchKeys: new UpdateStreamSettingValue<string> { Set = ["body"] },
            maxQueryRange: 120);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Put);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/streams/my-stream/settings");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"partition_keys\":{\"set\":[\"a\",\"b\"],\"add\":[\"c\"]}");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"full_text_search_keys\":{\"set\":[\"body\"]}");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"max_query_range\":120");
    }

    [Fact]
    public async Task Should_Delete_Stream()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"stream deleted\"}");
        var client = CreateClient();

        var result = await client.DeleteStreamAsync("my-stream", StreamType.Traces, deleteAll: true);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Delete);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/streams/my-stream?type=traces&delete_all=true");
        result.Message.ShouldBe("stream deleted");
    }

    [Fact]
    public async Task Should_Delete_Stream_Data_By_Time_Range()
    {
        HttpMessageHandler.EnqueueOk("{\"id\":\"job-1\"}");
        var client = CreateClient();

        var startTime = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var endTime = new DateTime(2024, 1, 3, 3, 4, 5, DateTimeKind.Utc);

        var result = await client.DeleteStreamDataByTimeRangeAsync("my-stream", startTime, endTime);

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe(
            $"/api/default/streams/my-stream/data_by_time_range?start={ToMicroseconds(startTime)}&end={ToMicroseconds(endTime)}");
        result.Id.ShouldBe("job-1");
    }

    [Fact]
    public async Task Should_Get_Delete_Job_Status()
    {
        HttpMessageHandler.EnqueueOk("{\"id\":\"job-1\",\"status\":\"Completed\"}");
        var client = CreateClient();

        var result = await client.GetStreamDataByTimeRangeStatusAsync("my-stream", "job-1");

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/streams/my-stream/data_by_time_range/status/job-1");
        result.Status.ShouldBe("Completed");
    }

    [Fact]
    public async Task Should_Delete_Stream_Cache()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"cache deleted\"}");
        var client = CreateClient();

        var timestamp = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        await client.DeleteStreamCacheAsync("_all", StreamType.Logs, timestamp);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Delete);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe(
            $"/api/default/streams/_all/cache/results?type=logs&ts={ToMicroseconds(timestamp)}");
    }
}
