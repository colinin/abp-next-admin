using LINGYUN.Abp.OpenObserve.Models.Function;
using Shouldly;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.OpenObserve;

public class OpenObserveClient_User_Function_Tests : AbpOpenObserveTestBase
{
    [Fact]
    public async Task Should_Create_User()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"User saved successfully\"}");
        var client = CreateClient();

        var result = await client.CreateUserAsync(
            "user@abp.io",
            "complex#pass",
            firstName: "ming",
            lastName: "xing",
            role: "admin");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Post);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/users");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"email\":\"user@abp.io\"");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"password\":\"complex#pass\"");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"first_name\":\"ming\"");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"last_name\":\"xing\"");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"role\":\"admin\"");
        result.Message.ShouldBe("User saved successfully");
    }

    [Fact]
    public async Task Should_Update_User_Without_Password()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"User saved successfully\"}");
        var client = CreateClient();

        await client.UpdateUserAsync("user@abp.io", firstName: "ming2", role: "user");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Put);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/users/user%40abp.io");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"first_name\":\"ming2\"");
        HttpMessageHandler.LastRequest.Body!.ShouldNotContain("old_password");
        HttpMessageHandler.LastRequest.Body!.ShouldNotContain("new_password");
    }

    [Fact]
    public async Task Should_Add_User_To_Organization()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"User added to org successfully\"}");
        var client = CreateClient();

        var result = await client.AddUserToOrganizationAsync("user@abp.io", role: "admin");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Post);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/users/user%40abp.io");
        HttpMessageHandler.LastRequest.Body!.ShouldContain("\"role\":\"admin\"");
        result.Message.ShouldBe("User added to org successfully");
    }

    [Fact]
    public async Task Should_Remove_User_From_Organization()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"User removed from organization\"}");
        var client = CreateClient();

        var result = await client.RemoveUserFromOrganizationAsync("user@abp.io");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Delete);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/users/user%40abp.io");
        result.Message.ShouldBe("User removed from organization");
    }

    [Fact]
    public async Task Should_List_Users_From_Data_Property()
    {
        HttpMessageHandler.EnqueueOk(
            "{\"data\":[{\"email\":\"admin@abp.io\",\"first_name\":\"root\",\"last_name\":\"\",\"role\":\"root\",\"is_external\":false}]}");
        var client = CreateClient();

        var result = await client.GetUsersAsync();

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/users");
        result.Users.Length.ShouldBe(1);
        result.Users[0].Email.ShouldBe("admin@abp.io");
        result.Users[0].Role.ShouldBe("root");
    }

    [Fact]
    public async Task Should_List_Users_From_List_Property()
    {
        HttpMessageHandler.EnqueueOk("{\"list\":[{\"email\":\"user@abp.io\",\"role\":\"user\"}]}");
        var client = CreateClient();

        var result = await client.GetUsersAsync();

        result.Users.Length.ShouldBe(1);
        result.Users[0].Email.ShouldBe("user@abp.io");
    }

    [Fact]
    public async Task Should_Create_Function()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"Function saved successfully\"}");
        var client = CreateClient();

        var result = await client.CreateFunctionAsync(
            "my_func",
            ".new_field = \"abc\"",
            transType: FunctionTransType.Vrl,
            streams: [new FunctionStreamInfo { Stream = "log1", Order = 1, StreamType = "logs" }]);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Post);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/functions");

        var body = JsonNode.Parse(HttpMessageHandler.LastRequest.Body!)!;
        body["name"]!.GetValue<string>().ShouldBe("my_func");
        body["function"]!.GetValue<string>().ShouldBe(".new_field = \"abc\"");
        body["transType"]!.GetValue<int>().ShouldBe(0);
        body["streams"]![0]!["stream"]!.GetValue<string>().ShouldBe("log1");
        body["streams"]![0]!["order"]!.GetValue<int>().ShouldBe(1);
        body["streams"]![0]!["streamType"]!.GetValue<string>().ShouldBe("logs");
        result.Code.ShouldBe(200);
    }

    [Fact]
    public async Task Should_Update_Function()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"Function saved successfully\"}");
        var client = CreateClient();

        await client.UpdateFunctionAsync("my_func", ".old_field = \"new_value\"");

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Put);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/functions/my_func");
    }

    [Fact]
    public async Task Should_Delete_Function_With_Force()
    {
        HttpMessageHandler.EnqueueOk("{\"code\":200,\"message\":\"Function deleted\"}");
        var client = CreateClient();

        var result = await client.DeleteFunctionAsync("my_func", force: true);

        HttpMessageHandler.LastRequest.Method.ShouldBe(HttpMethod.Delete);
        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/functions/my_func?force=true");
        result.Message.ShouldBe("Function deleted");
    }

    [Fact]
    public async Task Should_List_Functions()
    {
        HttpMessageHandler.EnqueueOk(
            "{\"list\":[{\"function\":\"function(row) return row end\",\"name\":\"fun1\",\"params\":\"\",\"numArgs\":2,\"transType\":0,\"streams\":[{\"stream\":\"log1\",\"order\":1}],\"stream_name\":\"log1\"}]}");
        var client = CreateClient();

        var result = await client.GetFunctionsAsync();

        result.List.Length.ShouldBe(1);
        result.List[0].Name.ShouldBe("fun1");
        result.List[0].NumArgs.ShouldBe(2);
        result.List[0].TransType.ShouldBe(FunctionTransType.Vrl);
        result.List[0].Streams![0].Stream.ShouldBe("log1");
        result.List[0].ExtraProperties.ShouldNotBeNull();
        result.List[0].ExtraProperties!.ContainsKey("stream_name").ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Test_Function()
    {
        HttpMessageHandler.EnqueueOk("{\"results\":[{\"message\":\"ok\",\"event\":{\"new_field\":\"abc\"}}]}");
        var client = CreateClient();

        var result = await client.TestFunctionAsync(
            ".new_field = \"abc\"",
            [JsonNode.Parse("{\"old_field\":\"v\"}")!],
            FunctionTransType.Vrl);

        HttpMessageHandler.LastRequest.PathAndQuery.ShouldBe("/api/default/functions/test");

        var body = JsonNode.Parse(HttpMessageHandler.LastRequest.Body!)!;
        body["function"]!.GetValue<string>().ShouldBe(".new_field = \"abc\"");
        body["events"]![0]!["old_field"]!.GetValue<string>().ShouldBe("v");
        body["trans_type"]!.GetValue<int>().ShouldBe(0);
        result.Results.Length.ShouldBe(1);
        result.Results[0].Message.ShouldBe("ok");
    }
}
