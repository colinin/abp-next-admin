using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Xunit;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 从数据库 JSON 字符串解析渠道数据测试
/// </summary>
/// <remarks>
/// <para>
/// 前面的测试都是"内存中写入 → 读取"的同进程路径, 验证不到持久化往返。
/// 本组测试补上**真实链路**: 直接给出数据库里存的 JSON 字符串,
/// 还原为 <see cref="NotificationData"/> 后, 再走渠道侧解析与消息工厂,
/// 回答"从数据库取到数据后能否正常解析为渠道消息格式"。
/// </para>
/// <para>
/// 关键点: 属性包里的值在持久化后是 **JSON 字符串**(而非嵌套对象),
/// 且模板卡片的判别符 <c>card_type</c> 必须保留, 否则抽象类型无法还原。
/// </para>
/// </remarks>
public class WeChatWorkWebhookDbJsonChannelData_Tests
    : AbpNotificationsWebhookWeChatWorkTestsBase
{
    private const string Provider = WeChatWorkWebhookNotificationProvider.ProviderName;

    private AbpNotificationsWebhookWeChatWorkMessageOptions MessageOptions
        => GetRequiredService<IOptions<AbpNotificationsWebhookWeChatWorkMessageOptions>>().Value;

    private AbpNotificationsWebhookWeChatWorkOptions ChannelOptions
        => GetRequiredService<IOptions<AbpNotificationsWebhookWeChatWorkOptions>>().Value;

    // ---------- 1. 模板卡片(抽象类型 + 判别符) ----------

    /// <summary>
    /// 数据库中的模板卡片 JSON(转义形式, 与 EF 存的一致)
    /// </summary>
    private const string TemplateCardDbJson =
        "{" +
        "\"wx-work-hook:template_card\":\"{\\\"sub_title_text\\\":\\\"请查看日志\\\"," +
        "\\\"card_type\\\":\\\"text_notice\\\"," +
        "\\\"main_title\\\":{\\\"title\\\":\\\"构建失败\\\",\\\"desc\\\":\\\"构建 #1234 失败\\\"}," +
        "\\\"horizontal_content_list\\\":[{\\\"keyname\\\":\\\"分支\\\",\\\"value\\\":\\\"main\\\"}]," +
        "\\\"card_action\\\":{\\\"type\\\":1,\\\"url\\\":\\\"https://example.com/build/1\\\"}}\"" +
        "}";

    [Fact]
    public void Should_Restore_TemplateCard_From_Db_Json()
    {
        var data = RestoreFromDbJson(TemplateCardDbJson);

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookTemplateCardChannelData>(data);

        payload.ShouldNotBeNull();
        payload!.TemplateCard.ShouldNotBeNull();

        // 抽象类型按 card_type 还原为具体类型
        var card = payload.TemplateCard.ShouldBeOfType<WebhookTextNoticeCardMessage>();
        card.CardType.ShouldBe("text_notice");
        card.MainTitle!.Title.ShouldBe("构建失败");
        card.MainTitle!.Description.ShouldBe("构建 #1234 失败");
        card.SubTitleText.ShouldBe("请查看日志");
        card.Action!.Url.ShouldBe("https://example.com/build/1");

        // 嵌套列表也要完整还原
        card.HorizontalContents.ShouldNotBeNull();
        card.HorizontalContents!.Count.ShouldBe(1);
        card.HorizontalContents[0].KeyName.ShouldBe("分支");
        card.HorizontalContents[0].Value.ShouldBe("main");
    }

    [Fact]
    public void Should_Build_TemplateCard_Message_From_Db_Json()
    {
        var data = RestoreFromDbJson(TemplateCardDbJson);

        var message = BuildMessage(Provider, data);

        message.MsgType.ShouldBe("template_card");

        var card = message.ShouldBeOfType<WeChatWorkWebhookTemplateCardMessage>();
        card.TemplateCard.ShouldBeOfType<WebhookTextNoticeCardMessage>()
            .MainTitle!.Title.ShouldBe("构建失败");
    }

    // ---------- 2. 标量载荷 ----------

    private const string TextDbJson =
        "{\"wx-work-hook:text\":\"构建 #1234 失败\"}";

    [Fact]
    public void Should_Restore_Text_From_Db_Json()
    {
        var data = RestoreFromDbJson(TextDbJson);

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookTextChannelData>(data);

        payload.ShouldNotBeNull();
        payload!.Content.ShouldBe("构建 #1234 失败");

        BuildMessage(Provider, data)
            .ShouldBeOfType<WeChatWorkWebhookTextMessage>()
            .Text.Content.ShouldBe("构建 #1234 失败");
    }

    // ---------- 3. 基础参数与渠道数据共存 ----------

    [Fact]
    public void Should_Restore_Standard_Data_Alongside_Channel_Data()
    {
        // 真实库中既有通知基础参数, 也有渠道数据
        var data = RestoreFromDbJson(
            "{\"title\":\"构建失败\",\"message\":\"构建 #1234 失败\",\"formUser\":\"ci\"," +
            "\"wx-work-hook:text\":\"兜底文本\"}");

        data.TryGetData("title").ShouldBe("构建失败");
        data.TryGetData("formUser").ShouldBe("ci");

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookTextChannelData>(data);
        payload.ShouldNotBeNull();
        payload!.Content.ShouldBe("兜底文本");
    }

    // ---------- 4. 无渠道数据时不误解析 ----------

    [Fact]
    public void Should_Resolve_Nothing_When_Db_Json_Has_No_Channel_Data()
    {
        var data = RestoreFromDbJson(
            "{\"title\":\"构建失败\",\"message\":\"构建 #1234 失败\"}");

        ChannelDataResolver.Resolve(Provider, data).ShouldBeEmpty();
    }

    // ---------- 5. 载荷非法时的行为 ----------

    [Fact]
    public void Should_Report_Declared_But_Resolve_Nothing_When_Db_Json_Payload_Is_Invalid()
    {
        // 模板卡片 JSON 缺少判别符 card_type, 抽象类型无法还原
        var data = RestoreFromDbJson(
            "{\"wx-work-hook:template_card\":\"{\\\"main_title\\\":{\\\"title\\\":\\\"t\\\"}}\"}");

        // 契约: 生产者确实写了该键 → IsDeclared 为 true,
        // 但解析失败 → Resolve 返回空(仅记日志), 不抛异常
        ChannelDataResolver.IsDeclared(data, typeof(WeChatWorkWebhookTemplateCardChannelData)).ShouldBeTrue();
        ChannelDataResolver.Resolve(Provider, data).ShouldBeEmpty();

        // 诊断信息仍能给出实际命中的键, 便于排查
        ChannelDataResolver.GetKey(data, typeof(WeChatWorkWebhookTemplateCardChannelData))
            .ShouldBe(WeChatWorkWebhookNotificationDataKeys.TemplateCard);
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

    private WeChatWorkWebhookMessage BuildMessage(string provider, NotificationData data)
    {
        var payload = ChannelDataResolver.Resolve(provider, data);
        payload.ShouldNotBeEmpty();

        return MessageOptions.Create(new WeChatWorkWebhookMessageFactoryContext(
            new NotificationInfo { Name = "Test", ContentType = NotificationContentType.Text },
            new NotificationStandardData("标题", "内容"),
            (WeChatWorkWebhookChannelData)payload[0],
            ChannelOptions)).Message;
    }
}
