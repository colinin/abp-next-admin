using LINGYUN.Abp.WeChat.MiniProgram.Messages;
using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Json;
using Xunit;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram;

/// <summary>
/// 从数据库 JSON 字符串解析渠道数据测试
/// </summary>
/// <remarks>
/// <para>
/// 前面的测试是"内存中写入 → 读取"的同进程路径, 验证不到持久化往返。
/// 本组测试补上**真实链路**: 直接给出数据库里存的 JSON 字符串,
/// 还原为 <see cref="NotificationData"/> 后, 再走渠道侧解析与真实发布,
/// 回答"从数据库取到数据后能否正常解析为渠道消息格式"。
/// </para>
/// <para>
/// 小程序渠道数据都是标量, 因此库中值为原始类型(而非 JSON 字符串)。
/// </para>
/// </remarks>
public class WeChatMiniProgramDbJsonChannelData_Tests
    : AbpNotificationsWeChatMiniProgramTestsBase
{
    private const string Provider = WeChatMiniProgramNotificationPublishProvider.ProviderName;

    private FakeSubscribeMessager Messager
        => (FakeSubscribeMessager)GetRequiredService<ISubscribeMessager>();

    // ---------- 1. 带前缀键 ----------

    [Fact]
    public void Should_Restore_Channel_Data_From_Db_Json()
    {
        var data = RestoreFromDbJson(
            "{\"title\":\"订单状态\",\"message\":\"您的订单已发货\"," +
            "\"wx-mp:template_id\":\"TPL_DB\"," +
            "\"wx-mp:redirect_page\":\"pages/order/detail\"," +
            "\"wx-mp:weapp_state\":\"developer\"," +
            "\"wx-mp:weapp_language\":\"en_US\"}");

        var payload = ChannelDataResolver.Resolve<WeChatMiniProgramSubscribeChannelData>(data);

        payload.ShouldNotBeNull();
        payload!.TemplateId.ShouldBe("TPL_DB");
        payload.RedirectPage.ShouldBe("pages/order/detail");
        payload.WeAppState.ShouldBe("developer");
        payload.WeAppLanguage.ShouldBe("en_US");
    }

    // ---------- 2. 从数据库 JSON 一路发到渠道(端到端) ----------

    [Fact]
    public async Task Should_Send_With_Data_Restored_From_Db_Json()
    {
        Messager.Clear();

        var data = RestoreFromDbJson(
            "{\"wx-mp:template_id\":\"TPL_DB_SEND\"," +
            "\"wx-mp:redirect_page\":\"pages/a/b\"," +
            "\"wx-mp:weapp_state\":\"formal\"}");

        await PublishAsync(data, NewUser());

        // 从库中还原的数据必须能真正发出去
        Messager.SentTemplates.Count.ShouldBe(1);

        var sent = Messager.SentTemplates[0];
        sent.TemplateId.ShouldBe("TPL_DB_SEND");
        sent.Page.ShouldBe("pages/a/b");
        sent.State.ShouldBe("formal");
    }

    // ---------- 3. 历史遗留无前缀键 ----------

    /// <remarks>
    /// <para>
    /// 历史键不在渠道数据解析器的匹配范围内, 而是由渠道 Provider 在解析时回退读取,
    /// 因此这里必须走真实发布路径验证。
    /// </para>
    /// <para>
    /// 键名以 <b>JSON 字面量</b>书写(而非引用已弃用的
    /// <c>WeChatMiniProgramNotificationDataKeys.Legacy</c>):
    /// 既避免引用弃用成员, 也更贴近"数据库里存的就是这些字符串"的事实。
    /// </para>
    /// </remarks>
    [Fact]
    public async Task Should_Restore_Legacy_Keys_From_Db_Json()
    {
        Messager.Clear();

        // 早期版本写入的无前缀键, 已安装的生产者可能仍在写
        var data = RestoreFromDbJson(
            "{\"TemplateId\":\"TPL_LEGACY\",\"RedirectPage\":\"pages/legacy\"}");

        await PublishAsync(data, NewUser());

        Messager.SentTemplates.Count.ShouldBe(1);
        Messager.SentTemplates[0].TemplateId.ShouldBe("TPL_LEGACY");
        Messager.SentTemplates[0].Page.ShouldBe("pages/legacy");
    }

    [Fact]
    public async Task Should_Prefer_Prefixed_Key_Over_Legacy_Key_From_Db_Json()
    {
        Messager.Clear();

        var data = RestoreFromDbJson(
            "{\"TemplateId\":\"TPL_LEGACY\",\"wx-mp:template_id\":\"TPL_NEW\"}");

        // 渠道数据解析器只认带前缀的键
        ChannelDataResolver.Resolve<WeChatMiniProgramSubscribeChannelData>(data)!
            .TemplateId.ShouldBe("TPL_NEW");

        // 真实发布时带前缀的键必须优先
        await PublishAsync(data, NewUser());

        Messager.SentTemplates[0].TemplateId.ShouldBe("TPL_NEW");
    }

    // ---------- 4. 无渠道数据时不误解析 ----------

    [Fact]
    public void Should_Resolve_Nothing_When_Db_Json_Has_No_Channel_Data()
    {
        var data = RestoreFromDbJson(
            "{\"title\":\"订单状态\",\"message\":\"您的订单已发货\"}");

        ChannelDataResolver.Resolve(Provider, data).ShouldBeEmpty();
    }

    // ---------- 辅助 ----------

    /// <summary>
    /// 模拟"从数据库取出 JSON 字符串"后还原通知数据
    /// </summary>
    /// <remarks>
    /// 与 EF 的属性包值转换器一致: 属性包整体是一个扁平 JSON 对象。
    /// </remarks>
    private NotificationData RestoreFromDbJson(string dbJson)
    {
        var extraProperties = JsonSerializer.Deserialize<ExtraPropertyDictionary>(dbJson);

        return new NotificationData { ExtraProperties = extraProperties };
    }

    private static UserIdentifier NewUser() => new(Guid.NewGuid(), "tester");

    private async Task PublishAsync(NotificationData data, UserIdentifier user)
    {
        var provider = GetRequiredService<WeChatMiniProgramNotificationPublishProvider>();

        var notification = new NotificationInfo
        {
            Name = "Test",
            ContentType = NotificationContentType.Text,
            Data = data
        };
        notification.SetId(1);

        await provider.PublishAsync(new NotificationPublishContext(notification, new[] { user }));
    }
}
