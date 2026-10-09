using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.User;
using LINGYUN.Abp.OpenObserve.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace LINGYUN.Abp.OpenObserve;

public class DefaultOpenObserveSerializer_Tests : AbpOpenObserveTestBase
{
    private IOpenObserveSerializer CreateSerializer()
    {
        return ServiceProvider.GetRequiredService<IOpenObserveSerializer>();
    }

    [Fact]
    public void Should_Resolve_Default_Implementation_From_Di()
    {
        CreateSerializer().ShouldBeOfType<DefaultOpenObserveSerializer>();
    }

    [Fact]
    public void Should_Ignore_Null_Fields_When_Serializing_Models()
    {
        var json = CreateSerializer().Serialize(new UpdateUserRequest
        {
            FirstName = "ming2",
            Role = "user",
        });

        json.ShouldBe("{\"first_name\":\"ming2\",\"role\":\"user\"}");
    }

    [Fact]
    public void Should_Not_Affect_Types_Outside_Models()
    {
        var json = CreateSerializer().Serialize(new { name = "my_func", order = (int?)null });

        // 非本库命名空间的类型不参与 null 忽略
        json.ShouldBe("{\"name\":\"my_func\",\"order\":null}");
    }

    [Fact]
    public void Should_Use_Relaxed_Encoding_When_Serializing()
    {
        var json = CreateSerializer().Serialize(new { message = "中文 \"quoted\"" });

        json.ShouldContain("中文");
        json.ShouldContain("\\\"quoted\\\"");
        json.ShouldNotContain("\\u");
    }

    [Fact]
    public void Should_Respect_JsonPropertyName_When_Serializing()
    {
        var json = CreateSerializer().Serialize(new AddUserToOrganizationRequest { Role = "admin" });

        json.ShouldBe("{\"role\":\"admin\"}");
    }

    [Fact]
    public void Should_Deserialize_Response()
    {
        var model = CreateSerializer().Deserialize<OpenObserveCodeMessageResponse>("{\"code\":200,\"message\":\"ok\"}");

        model.ShouldNotBeNull();
        model!.Code.ShouldBe(200);
        model.Message.ShouldBe("ok");
    }

    [Fact]
    public void Should_Return_False_When_TryDeserialize_Fails()
    {
        var serializer = CreateSerializer();

        serializer.TryDeserialize<OpenObserveCodeMessageResponse>("not-a-json", out var invalid).ShouldBeFalse();
        invalid.ShouldBeNull();

        serializer.TryDeserialize<OpenObserveCodeMessageResponse>(string.Empty, out var empty).ShouldBeFalse();
        empty.ShouldBeNull();
    }

    [Fact]
    public void Should_Return_True_When_TryDeserialize_Succeeds()
    {
        var serializer = CreateSerializer();

        serializer.TryDeserialize<OpenObserveErrorResponse>(
            "{\"code\":20004,\"message\":\"unknown field 'servce'\",\"suggestions\":[\"service\"]}",
            out var error).ShouldBeTrue();

        error.ShouldNotBeNull();
        error!.Code.ShouldBe(20004);
        error.Suggestions.ShouldBe(["service"]);
    }
}
