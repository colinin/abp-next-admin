using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信应用消息渠道数据测试
/// </summary>
/// <remarks>
/// 统一写法: 先 new 消息对象 → 通过 <c>IJsonSerializer</c> 序列化写入通知数据 →
/// 再用渠道侧解析器验证还原结果。
/// </remarks>
public class WeChatWorkChannelData_Tests : AbpNotificationsWeChatWorkTestsBase
{
    // ---------- 文本卡片 ----------

    [Fact]
    public void Should_Resolve_TextCard_Message()
    {
        var message = new TextCardMessage(
            title: "审批通过",
            description: "您的请假申请已通过",
            url: "https://example.com/leave/123",
            buttonText: "去查看");

        var payload = ChannelDataResolver.Resolve<WeChatWorkTextCardChannelData>(
            WriteChannelData(new WeChatWorkTextCardChannelData { TextCard = message }));

        payload.ShouldNotBeNull();
        payload!.TextCard.ShouldNotBeNull();

        // 渠道侧还原出的消息必须与生产者 new 出来的对象逐字段一致
        payload.TextCard!.Title.ShouldBe(message.Title);
        payload.TextCard.Description.ShouldBe(message.Description);
        payload.TextCard.Url.ShouldBe(message.Url);
        payload.TextCard.ButtonText.ShouldBe(message.ButtonText);
    }

    [Fact]
    public void Should_Serialize_TextCard_By_Json_Contract()
    {
        var json = Serialize(new TextCardMessage("标题", "描述", "https://example.com"));

        // 序列化契约: 字段名必须是企业微信要求的 snake_case
        json.ShouldContain("\"title\"");
        json.ShouldContain("\"description\"");
        json.ShouldContain("\"url\"");
    }

    // ---------- 小程序通知 ----------

    [Fact]
    public void Should_Resolve_MiniProgram_Message()
    {
        var message = new MiniProgramMessage(
            appId: "wx1234567890",
            title: "订单已发货",
            page: "pages/order/detail?id=123",
            description: "点击查看物流详情")
        {
            EmphasisFirstItem = true,
            ContentItems = new List<MiniProgramContent>
            {
                new MiniProgramContent("订单号", "SO20251001001")
            }
        };

        var payload = ChannelDataResolver.Resolve<WeChatWorkMiniProgramNoticeChannelData>(
            WriteChannelData(new WeChatWorkMiniProgramNoticeChannelData { MiniProgram = message }));

        payload.ShouldNotBeNull();
        payload!.MiniProgram.ShouldNotBeNull();
        payload.MiniProgram!.AppId.ShouldBe("wx1234567890");
        payload.MiniProgram.Title.ShouldBe("订单已发货");
        payload.MiniProgram.Page.ShouldBe("pages/order/detail?id=123");
        payload.MiniProgram.EmphasisFirstItem.ShouldBe(true);
        payload.MiniProgram.ContentItems.ShouldNotBeNull();
        payload.MiniProgram.ContentItems!.Count.ShouldBe(1);
        payload.MiniProgram.ContentItems[0].Key.ShouldBe("订单号");
        payload.MiniProgram.ContentItems[0].Value.ShouldBe("SO20251001001");
    }

    // ---------- 图文消息 ----------

    [Fact]
    public void Should_Resolve_MpNew_Message()
    {
        var message = new MpNewMessagePayload(new List<MpNewMessage>
        {
            new MpNewMessage(
                title: "月度报告",
                thumbMediaId: "MEDIA_ID",
                content: "<p>报告正文</p>",
                author: "运营部",
                contentSourceUrl: "https://example.com/report",
                description: "2025年10月经营月报")
        });

        var payload = ChannelDataResolver.Resolve<WeChatWorkMpNewChannelData>(
            WriteChannelData(new WeChatWorkMpNewChannelData { MpNewMessage = message }));

        payload.ShouldNotBeNull();
        payload!.MpNewMessage.ShouldNotBeNull();
        payload.MpNewMessage!.Articles.Count.ShouldBe(1);

        var article = payload.MpNewMessage.Articles[0];
        article.Title.ShouldBe("月度报告");
        article.ThumbMediaId.ShouldBe("MEDIA_ID");
        article.Content.ShouldBe("<p>报告正文</p>");
        article.Author.ShouldBe("运营部");
        article.ContentSourceUrl.ShouldBe("https://example.com/report");
        article.Description.ShouldBe("2025年10月经营月报");
    }

    // ---------- 视频 / 语音 / 文件 ----------

    [Fact]
    public void Should_Resolve_Video_Message()
    {
        var message = new VideoMessage("MEDIA_ID", "培训视频", "新人入职培训");

        var payload = ChannelDataResolver.Resolve<WeChatWorkVideoChannelData>(
            WriteChannelData(new WeChatWorkVideoChannelData { Video = message }));

        payload.ShouldNotBeNull();
        payload!.Video.ShouldNotBeNull();
        payload.Video!.MediaId.ShouldBe("MEDIA_ID");
        payload.Video.Title.ShouldBe("培训视频");
        payload.Video.Description.ShouldBe("新人入职培训");
    }

    [Fact]
    public void Should_Resolve_Voice_Message()
    {
        var message = new MediaMessage("VOICE_MEDIA_ID");

        var payload = ChannelDataResolver.Resolve<WeChatWorkVoiceChannelData>(
            WriteChannelData(new WeChatWorkVoiceChannelData { Voice = message }));

        payload.ShouldNotBeNull();
        payload!.Voice.ShouldNotBeNull();
        payload.Voice!.MediaId.ShouldBe("VOICE_MEDIA_ID");
    }

    [Fact]
    public void Should_Resolve_File_Message()
    {
        var message = new MediaMessage("FILE_MEDIA_ID");

        var payload = ChannelDataResolver.Resolve<WeChatWorkFileChannelData>(
            WriteChannelData(new WeChatWorkFileChannelData { File = message }));

        payload.ShouldNotBeNull();
        payload!.File.ShouldNotBeNull();
        payload.File!.MediaId.ShouldBe("FILE_MEDIA_ID");
    }

    // ---------- 模板卡片(抽象类型多态) ----------

    [Fact]
    public void Should_Resolve_TemplateCard_Message()
    {
        var message = new TextTemplateCard(
            new TemplateCardCardAction("https://example.com/approval/123"))
        {
            MainTitle = new TemplateCardMainTitle("请假通过通知", "您的请假申请已通过"),
            SubTitle = "请及时处理后续交接事项"
        };

        var payload = ChannelDataResolver.Resolve<WeChatWorkTemplateCardChannelData>(
            WriteChannelData(new WeChatWorkTemplateCardChannelData { TemplateCard = message }));

        payload.ShouldNotBeNull();
        payload!.TemplateCard.ShouldNotBeNull();

        // 抽象基类必须能还原为具体卡片类型
        payload.TemplateCard.ShouldBeOfType<TextTemplateCard>();

        var card = (TextTemplateCard)payload.TemplateCard!;
        card.MainTitle!.Title.ShouldBe("请假通过通知");
        card.SubTitle.ShouldBe("请及时处理后续交接事项");
        card.CardAction.Url.ShouldBe("https://example.com/approval/123");
    }

    // ---------- 投递选项与载荷共存 ----------

    [Fact]
    public void Should_Resolve_Payload_Along_With_Delivery_Options()
    {
        var data = WriteChannelData(new WeChatWorkTextCardChannelData
        {
            TextCard = new TextCardMessage("标题", "描述", "https://example.com")
        });

        data.TrySetData(WeChatWorkNotificationDataKeys.Safe, true);
        data.TrySetData(WeChatWorkNotificationDataKeys.ToParty, "1|2");
        data.TrySetData(WeChatWorkNotificationDataKeys.DuplicateCheckInterval, 600);

        // 载荷
        ChannelDataResolver.Resolve<WeChatWorkTextCardChannelData>(data).ShouldNotBeNull();

        // 标量选项
        ChannelDataResolver.GetValue(data, WeChatWorkNotificationDataKeys.Safe, false).ShouldBeTrue();
        ChannelDataResolver.GetValue(data, WeChatWorkNotificationDataKeys.DuplicateCheckInterval, 1800)
            .ShouldBe(600);
        ChannelDataResolver.GetOrNull(data, WeChatWorkNotificationDataKeys.ToParty).ShouldBe("1|2");
    }

    [Fact]
    public void Should_Require_Only_Its_Own_Key_To_Be_Declared()
    {
        // 只写一个载荷键, 不得误命中其它载荷类型
        var data = WriteChannelData(new WeChatWorkVideoChannelData
        {
            Video = new VideoMessage("MEDIA_ID")
        });

        ChannelDataResolver.IsDeclared(data, typeof(WeChatWorkVideoChannelData)).ShouldBeTrue();
        ChannelDataResolver.IsDeclared(data, typeof(WeChatWorkTextCardChannelData)).ShouldBeFalse();
        ChannelDataResolver.IsDeclared(data, typeof(WeChatWorkFileChannelData)).ShouldBeFalse();
        ChannelDataResolver.IsDeclared(data, typeof(WeChatWorkTemplateCardChannelData)).ShouldBeFalse();
    }

    // ---------- 多个载荷共存: 按 Priority 排序 ----------

    [Fact]
    public void Should_Resolve_All_Payloads_In_Priority_Order()
    {
        var data = WriteChannelData(new WeChatWorkTextCardChannelData
        {
            TextCard = new TextCardMessage("标题", "描述", "https://example.com")
        });
        // 合并另一个载荷(不同键, 互不冲突)
        MergeChannelData(data, new WeChatWorkMiniProgramNoticeChannelData
        {
            MiniProgram = new MiniProgramMessage("appid", "标题")
        });

        var payloads = ChannelDataResolver.Resolve(
            WeChatWorkNotificationPublishProvider.ProviderName,
            data);

        payloads.Count.ShouldBe(2);
        // Priority: 小程序通知 100 < 文本卡片 400
        payloads[0].ShouldBeOfType<WeChatWorkMiniProgramNoticeChannelData>();
        payloads[1].ShouldBeOfType<WeChatWorkTextCardChannelData>();
    }

    [Fact]
    public void Should_Declare_Every_Registered_Payload_Kind()
    {
        var types = ChannelDataResolver.GetPayloadTypes(WeChatWorkNotificationPublishProvider.ProviderName);

        types.Count.ShouldBe(7);
        types.ShouldContain(typeof(WeChatWorkMiniProgramNoticeChannelData));
        types.ShouldContain(typeof(WeChatWorkMpNewChannelData));
        types.ShouldContain(typeof(WeChatWorkTemplateCardChannelData));
        types.ShouldContain(typeof(WeChatWorkTextCardChannelData));
        types.ShouldContain(typeof(WeChatWorkVideoChannelData));
        types.ShouldContain(typeof(WeChatWorkVoiceChannelData));
        types.ShouldContain(typeof(WeChatWorkFileChannelData));
    }
}
