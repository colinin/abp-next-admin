using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.Ingestion;
using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.OpenObserve;

public class OpenObserveClient_Ingestion_Tests : AbpOpenObserveTestBase
{
    [Fact]
    public async Task Should_Ingest_Json_Array_Without_Extra_Nesting()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"status\":[{\"name\":\"my-stream\",\"successful\":2,\"failed\":0}]}");
        var client = CreateClient();

        var records = new[] { new { message = "one" }, new { message = "two" } };

        var result = await client.JsonRequestAsync("default", "my-stream", records);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Post);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/my-stream/_json");
        HttpMessageHandler.LastRequest.Body!.ShouldBe("[{\"message\":\"one\"},{\"message\":\"two\"}]");
        HttpMessageHandler.LastRequest.Body!.StartsWith("[[").ShouldBeFalse("请求体不应被额外包裹一层数组");
        result.Status[0].Successful.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Ingest_Single_Record()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"status\":[{\"name\":\"my-stream\",\"successful\":1,\"failed\":0}]}");
        var client = CreateClient();

        await client.JsonRecordAsync("default", "my-stream", new { message = "single" });

        HttpMessageHandler.LastRequest.Body!.ShouldBe("[{\"message\":\"single\"}]");
    }

    [Fact]
    public async Task Should_Ingest_Bulk_As_NdJson_With_Action_Line()
    {
        HttpMessageHandler.EnqueueOk(
            "{\"took\":0,\"errors\":false,\"items\":[{\"index\":{\"_index\":\"my-stream\",\"_id\":\"1\",\"status\":200,\"result\":\"created\"}}]}");
        var client = CreateClient();

        var result = await client.JsonBulkAsync("default", "my-stream", new[] { new { message = "bulk" } });

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/_bulk");
        HttpMessageHandler.LastRequest.ContentType.ShouldBe(OpenObserveConstants.NdJsonContentType);

        var lines = HttpMessageHandler.LastRequest.Body!.Split('\n');
        lines[0].Trim().ShouldBe("{\"index\":{\"_index\":\"my-stream\"}}");
        lines[1].Trim().ShouldBe("{\"message\":\"bulk\"}");

        result.Errors.ShouldBeFalse();
        result.Items.Length.ShouldBe(1);
        result.Items[0]["index"].Status.ShouldBe(200);
        result.Items[0]["index"].IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Ingest_Multi_As_NdJson_Without_Action_Line()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"status\":[{\"name\":\"my-stream\",\"successful\":1,\"failed\":0}]}");
        var client = CreateClient();

        await client.JsonMultiAsync("default", "my-stream", new[] { new { message = "multi" } });

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/my-stream/_multi");
        HttpMessageHandler.LastRequest.ContentType.ShouldBe(OpenObserveConstants.NdJsonContentType);
        HttpMessageHandler.LastRequest.Body!.Trim().ShouldBe("{\"message\":\"multi\"}");
    }

    [Fact]
    public async Task Should_Throw_Ingestion_Exception_With_Stream_On_Error()
    {
        HttpMessageHandler.Enqueue(HttpStatusCode.BadRequest, "{\"code\":400,\"message\":\"SerdeJsonError# key must be a string\"}");
        var client = CreateClient();

        var exception = await Should.ThrowAsync<OpenObserveIngestionException>(async () =>
            await client.JsonRequestAsync("default", "my-stream", new[] { new { message = "x" } }));

        exception.Stream.ShouldBe("my-stream");
        exception.StatusCode.ShouldBe(400);
        exception.Code.ShouldBe(400);
        exception.Message.ShouldBe("SerdeJsonError# key must be a string");
    }

    [Fact]
    public async Task Should_Deserialize_Failed_Record_Reason()
    {
        HttpMessageHandler.EnqueueOk(
            "{\"code\":200,\"status\":[{\"name\":\"my-stream\",\"successful\":0,\"failed\":1,\"error\":\"flatten value must be an object\"}]}");
        var client = CreateClient();

        var result = await client.JsonRequestAsync("default", "my-stream", new[] { new { message = "x" } });

        result.Status[0].Failed.ShouldBe(1);
        result.Status[0].Error.ShouldBe("flatten value must be an object");
        result.Status[0].IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Serialize_Records_With_Timestamp()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"status\":[]}");
        var client = CreateClient();

        var records = new List<Dictionary<string, object?>>
        {
            new Dictionary<string, object?> { ["message"] = "x", ["_timestamp"] = 1700000000000000L },
        };

        await client.JsonRequestAsync("default", "my-stream", records);

        HttpMessageHandler.LastRequest.Body!.ShouldBe("[{\"message\":\"x\",\"_timestamp\":1700000000000000}]");
    }
}
