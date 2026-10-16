using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace LINGYUN.Abp.Notifications.ChannelData.Tests;

/// <summary>
/// 渠道数据(载荷)框架契约测试
/// </summary>
/// <remarks>
/// <para>
/// 本文件只测试**框架契约**本身: 弱类型转换、存在性判定、注册期校验、
/// 优先级排序、JSON 往返。测试用载荷是本文件内定义的 stub 类型。
/// </para>
/// <para>
/// **具体渠道**的数据测试在各自的测试项目中(先 new 消息对象 →
/// <c>IJsonSerializer</c> 序列化 → 渠道侧验证):
/// <list type="bullet">
/// <item><c>LINGYUN.Abp.Notifications.WeChat.Work.Tests</c></item>
/// <item><c>LINGYUN.Abp.Notifications.Webhook.WeChat.Work.Tests</c></item>
/// <item><c>LINGYUN.Abp.Notifications.WeChat.MiniProgram.Tests</c></item>
/// <item><c>LINGYUN.Abp.Notifications.WxPusher.Tests</c></item>
/// <item><c>LINGYUN.Abp.Notifications.PushPlus.Tests</c></item>
/// </list>
/// </para>
/// </remarks>
public class NotificationChannelData_Tests
{
    // ---------- 1. 弱类型转换 ----------

    [Theory]
    [InlineData((byte)1)]
    [InlineData(1)]
    [InlineData(1L)]
    [InlineData("1")]
    [InlineData("\"1\"")]
    [InlineData(true)]
    [InlineData("true")]
    public void Should_Coerce_All_Representations_To_Bool(object stored)
    {
        var data = new NotificationData();
        data.TrySetData("key", stored);

        CreateResolver().GetValue(data, "key", false).ShouldBeTrue();
    }

    [Fact]
    public void Should_Distinguish_Unset_From_Explicit_False()
    {
        var unset = new NotificationData();
        CreateResolver().GetValue<bool>(unset, "key").ShouldBeNull();

        var explicitFalse = new NotificationData();
        explicitFalse.TrySetData("key", 0);
        CreateResolver().GetValue<bool>(explicitFalse, "key").ShouldBe(false);
    }

    [Fact]
    public void Should_Read_Values_After_Json_RoundTrip()
    {
        // 通知数据在持久化/失败重试/跨进程投递后必然经历 JSON 往返
        var data = new NotificationData();
        data.TrySetData("key", (byte)1);

        var roundTripped = RoundTrip(data);
        roundTripped.ExtraProperties["key"].ShouldNotBeOfType<byte>();

        CreateResolver().GetValue(roundTripped, "key", false).ShouldBeTrue();
    }

    [Fact]
    public void Should_Match_Keys_Case_Insensitively()
    {
        var data = new NotificationData();
        data.TrySetData("my-stub:key", "value-1");

        var resolver = CreateResolver();

        // 精确命中
        resolver.GetOrNull(data, "my-stub:key").ShouldBe("value-1");
        // 任意大小写都应命中
        resolver.GetOrNull(data, "MY-STUB:KEY").ShouldBe("value-1");
        resolver.GetOrNull(data, "My-Stub:Key").ShouldBe("value-1");
    }

    // ---------- 2. 存在性判定(声明式) ----------

    [Fact]
    public void Should_Detect_Explicit_Payload()
    {
        var data = new NotificationData();
        ChannelData().Write(data, new PriorityStubA { A = "a" });

        CreateResolver().IsDeclared(data, typeof(PriorityStubA)).ShouldBeTrue();
        CreateResolver().IsDeclared(data, typeof(PriorityStubB)).ShouldBeFalse();
    }

    [Fact]
    public void Should_Not_Treat_Ignored_Properties_As_Payload()
    {
        var data = new NotificationData();
        // 只写被标记为"仅读取"的属性: 不得视为声明了载荷
        data.TrySetData("stubB:optional", "value");

        CreateResolver().IsDeclared(data, typeof(PriorityStubB)).ShouldBeFalse();
    }

    [Fact]
    public void Should_Apply_AnyDeclaredProperty_Presence_Mode()
    {
        var resolver = CreateResolver();

        var first = new NotificationData();
        first.TrySetData("stub-many:first", "v");
        resolver.IsDeclared(first, typeof(AnyDeclaredPropertyStub)).ShouldBeTrue();

        var second = new NotificationData();
        second.TrySetData("stub-many:second", "v");
        resolver.IsDeclared(second, typeof(AnyDeclaredPropertyStub)).ShouldBeTrue();

        // 声明了按键判定, 但一个键都没写 -> 不存在
        resolver.IsDeclared(new NotificationData(), typeof(AnyDeclaredPropertyStub)).ShouldBeFalse();
    }

    [Fact]
    public void Should_Ignore_Matching_For_Optional_Properties()
    {
        var data = new NotificationData();
        // 只写"仅读取"的属性 -> 不算载荷
        data.TrySetData("stubB:optional", "value");
        CreateResolver().IsDeclared(data, typeof(PriorityStubB)).ShouldBeFalse();

        // 写标识键 -> 算载荷
        var withIdentity = new NotificationData();
        withIdentity.TrySetData("stub:b", "value");
        CreateResolver().IsDeclared(withIdentity, typeof(PriorityStubB)).ShouldBeTrue();
    }

    [Fact]
    public void Should_Allow_Always_Present_Payload()
    {
        var resolver = CreateResolver();

        // Presence = None: 无条件存在
        resolver.IsDeclared(new NotificationData(), typeof(AlwaysPresentStub)).ShouldBeTrue();

        var payload = resolver.Resolve<AlwaysPresentStub>(new NotificationData());
        payload.ShouldNotBeNull();
    }

    // ---------- 3. 注册期校验 ----------

    [Fact]
    public void Should_Throw_When_Presence_Declared_Without_Matchable_Property()
    {
        var exception = Should.Throw<AbpNotificationChannelDataConfigurationException>(
            () => CreateOptions().Add<MissingMatchablePropertyStub>());

        exception.Message.ShouldContain("matchable property");
    }

    [Fact]
    public void Should_Reject_Missing_Channel_Data_Attribute()
    {
        var exception = Should.Throw<AbpNotificationChannelDataConfigurationException>(
            () => CreateOptions().Add<NoAttributeStub>());

        exception.Message.ShouldContain("NotificationChannelDataAttribute");
    }

    [Fact]
    public void Should_Reject_Duplicate_Priority_In_Same_Provider()
    {
        var options = CreateOptions();
        options.Add<PriorityStubA>();

        var exception = Should.Throw<AbpNotificationChannelDataConfigurationException>(
            () => options.Add<DuplicatePriorityStub>());

        exception.Message.ShouldContain("same priority");
    }

    // ---------- 4. 解析与排序 ----------

    [Fact]
    public void Should_Resolve_Payloads_In_Declared_Priority_Order()
    {
        var data = new NotificationData();
        ChannelData().Write(data, new PriorityStubB { B = "b" });
        ChannelData().Write(data, new PriorityStubA { A = "a" });

        var resolved = CreateResolver().Resolve(TestProvider, data);

        // 只声明了 A 与 B; AnyDeclaredPropertyStub 未写入, AlwaysPresentStub(Presence=None)
        // 无条件存在, 因此共 3 个
        resolved.Count.ShouldBe(3);
        // Priority: AlwaysPresentStub=400, A=100, B=200 -> 排序后 A, B, Always
        resolved[0].ShouldBeOfType<PriorityStubA>();
        resolved[1].ShouldBeOfType<PriorityStubB>();
        resolved[2].ShouldBeOfType<AlwaysPresentStub>();
    }

    [Fact]
    public void Should_Resolve_Payload_After_Json_RoundTrip()
    {
        var data = new NotificationData();
        ChannelData().Write(data, new PriorityStubA { A = "卡片标题" });

        // 复杂载荷以 JSON 字符串形式存储, 往返后仍必须能还原为强类型
        data.ExtraProperties["stub:a"].ShouldBeOfType<string>();

        var payload = CreateResolver().Resolve<PriorityStubA>(RoundTrip(data));

        payload.ShouldNotBeNull();
        payload!.A.ShouldBe("卡片标题");
    }

    [Fact]
    public void Should_Not_Throw_For_Invalid_Payload_But_Report_Declared()
    {
        var data = new NotificationData();
        // 复杂载荷以 JSON 字符串存储; 这里写入非法 JSON
        data.TrySetData("stub:json", "{ this is not json }");

        var resolver = CreateResolver();

        // IsDeclared 仍为 true(生产者确实写了), Resolve 返回 null(解析失败, 仅记日志)
        resolver.IsDeclared(data, typeof(JsonPayloadStub)).ShouldBeTrue();
        resolver.Resolve<JsonPayloadStub>(data).ShouldBeNull();
        // 诊断信息给出实际命中的键名
        resolver.GetKey(data, typeof(JsonPayloadStub)).ShouldBe("stub:json");
    }

    [Fact]
    public void Should_Resolve_Complex_Payload_After_Json_RoundTrip()
    {
        var data = new NotificationData();
        ChannelData().Write(data, new JsonPayloadStub
        {
            Payload = new StubPayload { Name = "标题", Count = 3 }
        });

        // 复杂类型以 JSON 字符串形式存储
        data.ExtraProperties["stub:json"].ShouldBeOfType<string>();

        var payload = CreateResolver().Resolve<JsonPayloadStub>(RoundTrip(data));

        payload.ShouldNotBeNull();
        payload!.Payload.ShouldNotBeNull();
        payload.Payload!.Name.ShouldBe("标题");
        payload.Payload.Count.ShouldBe(3);
    }

    // ---------- 5. 测试用载荷 ----------

    private const string TestProvider = "stub";

    [NotificationChannelData(TestProvider, Priority = 100)]
    private class PriorityStubA : NotificationChannelData
    {
        [NotificationChannelProperty("stub:a")]
        public string? A { get; set; }
    }

    [NotificationChannelData(TestProvider, Priority = 200)]
    private class PriorityStubB : NotificationChannelData
    {
        [NotificationChannelProperty("stub:b")]
        public string? B { get; set; }

        [NotificationChannelProperty("stubB:optional", IgnoreWhenMatching = true)]
        public string? Optional { get; set; }
    }

    [NotificationChannelData(TestProvider, Priority = 100)]
    private class DuplicatePriorityStub : NotificationChannelData
    {
        [NotificationChannelProperty("stub:dup")]
        public string? Value { get; set; }
    }

    [NotificationChannelData(TestProvider, Priority = 300,
        Presence = NotificationChannelDataPresenceMode.AnyDeclaredProperty)]
    private class AnyDeclaredPropertyStub : NotificationChannelData
    {
        [NotificationChannelProperty("stub-many:first")]
        public string? First { get; set; }

        [NotificationChannelProperty("stub-many:second")]
        public string? Second { get; set; }
    }

    [NotificationChannelData(TestProvider, Priority = 400,
        Presence = NotificationChannelDataPresenceMode.None)]
    private class AlwaysPresentStub : NotificationChannelData
    {
        [NotificationChannelProperty("stub-always:value")]
        public string? Value { get; set; }
    }

    [NotificationChannelData(TestProvider, Priority = 500,
        Presence = NotificationChannelDataPresenceMode.DeclaredProperties)]
    private class MissingMatchablePropertyStub : NotificationChannelData
    {
        // 没有任何带 [NotificationChannelProperty] 的属性
        public string? Value { get; set; }
    }

    /// <summary>
    /// 复杂载荷(以 JSON 字符串存储)
    /// </summary>
    [NotificationChannelData(TestProvider, Priority = 600)]
    private class JsonPayloadStub : NotificationChannelData
    {
        [NotificationChannelProperty("stub:json")]
        public StubPayload? Payload { get; set; }
    }

    private class StubPayload
    {
        public string? Name { get; set; }

        public int Count { get; set; }
    }

    private class NoAttributeStub : NotificationChannelData
    {
        public string? Value { get; set; }
    }

    // ---------- 辅助 ----------

    private static AbpNotificationChannelDataOptions CreateOptions()
    {
        // 选项是纯数据: 存在性判定为纯静态, 注册时无需解析任何服务
        return new AbpNotificationChannelDataOptions();
    }

    private static NotificationChannelDataResolver CreateResolver()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<INotificationChannelDataJsonSerializer,
            TestNotificationChannelDataJsonSerializer>();
        services.AddSingleton<INotificationChannelDataAccessor, NotificationChannelDataAccessor>();
        services.AddSingleton<INotificationChannelDataPresenceProvider, NotificationChannelDataPresenceProvider>();
        services.AddSingleton<INotificationChannelDataResolver, NotificationChannelDataResolver>();

        services.PostConfigure<AbpNotificationChannelDataOptions>(options =>
        {
            options.Add<PriorityStubA>();
            options.Add<PriorityStubB>();
            options.Add<AnyDeclaredPropertyStub>();
            options.Add<AlwaysPresentStub>();
            options.Add<JsonPayloadStub>();
        });

        var provider = services.BuildServiceProvider();

        return (NotificationChannelDataResolver)provider.GetRequiredService<INotificationChannelDataResolver>();
    }

    private static INotificationChannelDataSerializer ChannelData()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<INotificationChannelDataJsonSerializer,
            TestNotificationChannelDataJsonSerializer>();
        services.AddSingleton<INotificationChannelDataAccessor, NotificationChannelDataAccessor>();
        services.AddSingleton<INotificationChannelDataPresenceProvider, NotificationChannelDataPresenceProvider>();
        services.AddSingleton<INotificationChannelDataSerializer, NotificationChannelDataSerializer>();

        return services.BuildServiceProvider().GetRequiredService<INotificationChannelDataSerializer>();
    }

    private static NotificationData RoundTrip(NotificationData data)
    {
        var json = JsonSerializer.Serialize(data.ExtraProperties);

        return new NotificationData
        {
            ExtraProperties = JsonSerializer.Deserialize<Volo.Abp.Data.ExtraPropertyDictionary>(json)!
        };
    }
}
