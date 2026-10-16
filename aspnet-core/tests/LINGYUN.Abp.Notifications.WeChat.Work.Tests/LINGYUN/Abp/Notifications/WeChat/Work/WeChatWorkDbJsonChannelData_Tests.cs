using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Json;
using Xunit;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 从数据库 JSON 字符串解析渠道数据测试
/// </summary>
/// <remarks>
/// <para>
/// 前面的测试是"内存中写入 → 读取"的同进程路径, 验证不到持久化往返。
/// 本组测试补上**真实链路**: 直接给出数据库里存的 JSON 字符串,
/// 还原为 <see cref="NotificationData"/> 后, 再走渠道侧解析与消息工厂,
/// 回答"从数据库取到数据后能否正常解析为渠道消息格式"。
/// </para>
/// <para>
/// 关键点: 属性包里的复杂值为 **JSON 字符串**(而非嵌套对象),
/// 且模板卡片的判别符 <c>card_type</c> 必须保留, 否则抽象类型无法还原。
/// </para>
/// </remarks>
public class WeChatWorkDbJsonChannelData_Tests
    : AbpNotificationsWeChatWorkTestsBase
{
    private const string Provider = NotificationProviderNames.WechatWork;

    // ---------- 1. 模板卡片(抽象类型 + 判别符) ----------

    /// <summary>
    /// 数据库中的模板卡片 JSON(转义形式, 与 EF 存的一致)
    /// </summary>
    private const string TemplateCardDbJson =
        "{\"wx-work:template_card_message\":\"{" +
        "\\\"main_title\\\":{\\\"title\\\":\\\"请假通过通知\\\",\\\"desc\\\":\\\"您的请假申请已通过\\\"}," +
        "\\\"sub_title_text\\\":\\\"请及时处理后继交接事项\\\"," +
        "\\\"horizontal_content_list\\\":[{\\\"keyname\\\":\\\"单号\\\",\\\"value\\\":\\\"LV20251001\\\"}]," +
        "\\\"TemplateCardCardAction\\\":{\\\"type\\\":1,\\\"url\\\":\\\"https://example.com/approval/123\\\"}," +
        "\\\"card_type\\\":\\\"template_card\\\"}\"}";

    [Fact]
    public void Should_Restore_TemplateCard_From_Db_Json()
    {
        var data = RestoreFromDbJson(TemplateCardDbJson);

        var payload = ChannelDataResolver.Resolve<WeChatWorkTemplateCardChannelData>(data);

        payload.ShouldNotBeNull();
        payload!.TemplateCard.ShouldNotBeNull();

        // 抽象基类必须能还原为具体卡片类型
        var card = payload.TemplateCard.ShouldBeOfType<TextTemplateCard>();
        card.MainTitle!.Title.ShouldBe("请假通过通知");
        card.MainTitle!.Description.ShouldBe("您的请假申请已通过");
        card.SubTitle.ShouldBe("请及时处理后继交接事项");
        card.CardAction.Url.ShouldBe("https://example.com/approval/123");

        // 嵌套列表也要完整还原
        card.HorizontalContents.ShouldNotBeNull();
        card.HorizontalContents!.Count.ShouldBe(1);
        card.HorizontalContents[0].KeyName.ShouldBe("单号");
        card.HorizontalContents[0].Value.ShouldBe("LV20251001");
    }

    // ---------- 2. 文本卡片(基础类型) ----------

    [Fact]
    public void Should_Restore_TextCard_From_Db_Json()
    {
        var data = RestoreFromDbJson(
            "{\"wx-work:text_card_message\":\"{" +
            "\\\"title\\\":\\\"标题\\\",\\\"description\\\":\\\"描述\\\"," +
            "\\\"url\\\":\\\"https://example.com\\\",\\\"btntxt\\\":\\\"详情\\\"}\"}");

        var payload = ChannelDataResolver.Resolve<WeChatWorkTextCardChannelData>(data);

        payload.ShouldNotBeNull();
        payload!.TextCard.ShouldNotBeNull();
        payload.TextCard!.Title.ShouldBe("标题");
        payload.TextCard!.Url.ShouldBe("https://example.com");
    }

    // ---------- 3. 投递选项与渠道数据共存 ----------

    [Fact]
    public void Should_Restore_Delivery_Options_Alongside_Channel_Data()
    {
        // 真实库中既有渠道数据, 也有投递选项(safe / 重复检查间隔等)
        var data = RestoreFromDbJson(
            "{\"wx-work:safe\":true,\"wx-work:duplicate_check_interval\":600," +
            "\"wx-work:text_card_message\":\"{" +
            "\\\"title\\\":\\\"标题\\\",\\\"description\\\":\\\"描述\\\",\\\"url\\\":\\\"https://example.com\\\"}\"}");

        ChannelDataResolver.Resolve<WeChatWorkTextCardChannelData>(data).ShouldNotBeNull();

        // 投递选项为原始类型, 需按声明类型正确还原
        ChannelDataResolver.GetValue(data, WeChatWorkNotificationDataKeys.Safe, false).ShouldBeTrue();
        ChannelDataResolver.GetValue(data, WeChatWorkNotificationDataKeys.DuplicateCheckInterval, 0).ShouldBe(600);
    }

    // ---------- 4. 无渠道数据时不误解析 ----------

    [Fact]
    public void Should_Resolve_Nothing_When_Db_Json_Has_No_Channel_Data()
    {
        var data = RestoreFromDbJson(
            "{\"title\":\"标题\",\"message\":\"内容\",\"formUser\":\"ci\"}");

        ChannelDataResolver.Resolve(Provider, data).ShouldBeEmpty();
    }

    // ---------- 5. 载荷非法时的行为 ----------

    [Fact]
    public void Should_Report_Declared_But_Resolve_Nothing_When_Db_Json_Payload_Is_Invalid()
    {
        // 复杂渠道数据以 JSON 字符串存放, 这里写入无法反序列化的内容
        var data = RestoreFromDbJson(
            "{\"wx-work:template_card_message\":\"{ this is not json }\"}");

        // 契约: 键确实存在 → IsDeclared 为 true; 解析失败 → Resolve 返回空, 不抛异常
        ChannelDataResolver.IsDeclared(data, typeof(WeChatWorkTemplateCardChannelData)).ShouldBeTrue();
        ChannelDataResolver.Resolve(Provider, data).ShouldBeEmpty();

        // 诊断信息仍能给出实际命中的键, 便于排查
        ChannelDataResolver.GetKey(data, typeof(WeChatWorkTemplateCardChannelData))
            .ShouldBe(WeChatWorkNotificationDataKeys.TemplateCardMessage);
    }

    // ---------- 辅助 ----------

    /// <summary>
    /// 模拟"从数据库取出 JSON 字符串"后还原通知数据
    /// </summary>
    /// <remarks>
    /// 与 EF 的属性包值转换器一致: 属性包整体是一个扁平 JSON 对象,
    /// 复杂渠道数据在其中以 <b>转义后的 JSON 字符串</b> 存放。
    /// </remarks>
    private NotificationData RestoreFromDbJson(string dbJson)
    {
        var extraProperties = JsonSerializer.Deserialize<ExtraPropertyDictionary>(dbJson);

        return new NotificationData { ExtraProperties = extraProperties };
    }
}
