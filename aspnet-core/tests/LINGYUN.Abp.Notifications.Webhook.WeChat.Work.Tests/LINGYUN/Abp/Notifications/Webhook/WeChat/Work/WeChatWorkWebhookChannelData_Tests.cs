using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace LINGYUN.Abp.Notifications.Webhook.WeChat.Work;

/// <summary>
/// 企业微信 Webhook 渠道数据测试
/// </summary>
/// <remarks>
/// 统一写法: 先 new 消息对象 → 通过 <c>IJsonSerializer</c> 序列化写入通知数据 →
/// 再用渠道侧解析器 + 消息工厂验证最终载体。
/// </remarks>
public class WeChatWorkWebhookChannelData_Tests : AbpNotificationsWebhookWeChatWorkTestsBase
{
    private const string Provider = WeChatWorkWebhookNotificationProvider.ProviderName;

    private AbpNotificationsWebhookWeChatWorkMessageOptions MessageOptions
        => GetRequiredService<IOptions<AbpNotificationsWebhookWeChatWorkMessageOptions>>().Value;

    private AbpNotificationsWebhookWeChatWorkOptions ChannelOptions
        => GetRequiredService<IOptions<AbpNotificationsWebhookWeChatWorkOptions>>().Value;

    // ---------- 1. 解析 + 载体构造(端到端) ----------

    [Fact]
    public void Should_Resolve_Text_Payload_And_Build_Text_Message()
    {
        var data = WriteChannelData(new WeChatWorkWebhookTextChannelData
        {
            Content = "构建 #1234 失败"
        });

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookTextChannelData>(data);
        payload.ShouldNotBeNull();
        payload!.Content.ShouldBe("构建 #1234 失败");

        var message = BuildMessage(payload);
        message.MsgType.ShouldBe("text");

        var text = message.ShouldBeOfType<WeChatWorkWebhookTextMessage>();
        text.Text.Content.ShouldBe("构建 #1234 失败");
    }

    [Fact]
    public void Should_Resolve_Markdown_Payload_And_Build_Markdown_Message()
    {
        var data = WriteChannelData(new WeChatWorkWebhookMarkdownChannelData
        {
            Content = "**构建失败**"
        });

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookMarkdownChannelData>(data);
        payload.ShouldNotBeNull();

        var message = BuildMessage(payload!);
        message.MsgType.ShouldBe("markdown");
        message.ShouldBeOfType<WeChatWorkWebhookMarkdownMessage>()
            .Markdown.Content.ShouldBe("**构建失败**");
    }

    [Fact]
    public void Should_Resolve_MarkdownV2_Payload_And_Build_MarkdownV2_Message()
    {
        var data = WriteChannelData(new WeChatWorkWebhookMarkdownV2ChannelData
        {
            Content = "| a | b |\n|---|---|\n| 1 | 2 |"
        });

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookMarkdownV2ChannelData>(data);
        payload.ShouldNotBeNull();

        var message = BuildMessage(payload!);
        message.MsgType.ShouldBe("markdown_v2");
        message.ShouldBeOfType<WeChatWorkWebhookMarkdownV2Message>()
            .Markdown.Content.ShouldContain("| a | b |");
    }

    [Fact]
    public void Should_Resolve_Image_Payload_And_Build_Image_Message()
    {
        var image = new WebhookImageMessage("BASE64_DATA", "MD5_HASH");

        var data = WriteChannelData(new WeChatWorkWebhookImageChannelData { Image = image });

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookImageChannelData>(data);
        payload.ShouldNotBeNull();
        payload!.Image.ShouldNotBeNull();
        payload.Image!.Base64.ShouldBe("BASE64_DATA");
        payload.Image.Md5.ShouldBe("MD5_HASH");

        var message = BuildMessage(payload);
        message.MsgType.ShouldBe("image");
        message.ShouldBeOfType<WeChatWorkWebhookImageMessage>()
            .Image.Base64.ShouldBe("BASE64_DATA");
    }

    [Fact]
    public void Should_Resolve_News_Payload_And_Build_News_Message()
    {
        var news = new WebhookNewsMessage(new List<WebhookArticleMessage>
        {
            new WebhookArticleMessage(
                title: "月度报告",
                url: "https://example.com/report",
                description: "2025年10月",
                pictureUrl: "https://example.com/cover.png")
        });

        var data = WriteChannelData(new WeChatWorkWebhookNewsChannelData { News = news });

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookNewsChannelData>(data);
        payload.ShouldNotBeNull();
        payload!.News.ShouldNotBeNull();
        payload.News!.Articles.Count.ShouldBe(1);
        payload.News.Articles[0].Title.ShouldBe("月度报告");
        payload.News.Articles[0].Url.ShouldBe("https://example.com/report");
        payload.News.Articles[0].PictureUrl.ShouldBe("https://example.com/cover.png");

        var message = BuildMessage(payload);
        message.MsgType.ShouldBe("news");

        // 该载体原先因 WeChatWorkWebhookNewsMessage 为 internal 而无法使用
        var newsMessage = message.ShouldBeOfType<WeChatWorkWebhookNewsMessage>();
        newsMessage.News.Articles[0].Title.ShouldBe("月度报告");
    }

    [Fact]
    public void Should_Resolve_File_Payload_And_Build_File_Message()
    {
        var data = WriteChannelData(new WeChatWorkWebhookFileChannelData
        {
            File = new WebhookFileMessage("FILE_MEDIA_ID")
        });

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookFileChannelData>(data);
        payload.ShouldNotBeNull();

        var message = BuildMessage(payload!);
        message.MsgType.ShouldBe("file");
        message.ShouldBeOfType<WeChatWorkWebhookFileMessage>()
            .File.MediaId.ShouldBe("FILE_MEDIA_ID");
    }

    [Fact]
    public void Should_Resolve_Voice_Payload_And_Build_Voice_Message()
    {
        var data = WriteChannelData(new WeChatWorkWebhookVoiceChannelData
        {
            Voice = new WebhookVoiceMessage("VOICE_MEDIA_ID")
        });

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookVoiceChannelData>(data);
        payload.ShouldNotBeNull();

        var message = BuildMessage(payload!);
        message.MsgType.ShouldBe("voice");
        message.ShouldBeOfType<WeChatWorkWebhookVoiceMessage>()
            .Voice.MediaId.ShouldBe("VOICE_MEDIA_ID");
    }

    // ---------- 2. 模板卡片(抽象类型多态) ----------

    [Fact]
    public void Should_Resolve_TextNoticeCard_And_Build_TemplateCard_Message()
    {
        var card = new WebhookTextNoticeCardMessage(
            action: WebhookTemplateCardAction.Link("https://example.com/build/1234"),
            mainTitle: new WebhookTemplateCardMainTitle("构建失败", "请及时处理"),
            subTitleText: "构建 #1234 于 15:30 失败",
            horizontalContents: new List<WebhookTemplateCardHorizontalContent>
            {
                WebhookTemplateCardHorizontalContent.Default("分支", "main")
            });

        var data = WriteChannelData(new WeChatWorkWebhookTemplateCardChannelData { TemplateCard = card });

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookTemplateCardChannelData>(data);
        payload.ShouldNotBeNull();
        payload!.TemplateCard.ShouldNotBeNull();

        // 抽象基类必须还原为具体卡片类型
        var restored = payload.TemplateCard.ShouldBeOfType<WebhookTextNoticeCardMessage>();
        restored.CardType.ShouldBe("text_notice");
        restored.SubTitleText.ShouldBe("构建 #1234 于 15:30 失败");
        restored.Action!.Url.ShouldBe("https://example.com/build/1234");
        restored.MainTitle!.Title.ShouldBe("构建失败");
        restored.HorizontalContents!.Count.ShouldBe(1);

        var message = BuildMessage(payload);
        message.MsgType.ShouldBe("template_card");
        message.ShouldBeOfType<WeChatWorkWebhookTemplateCardMessage>()
            .TemplateCard.ShouldBeOfType<WebhookTextNoticeCardMessage>();
    }

    [Fact]
    public void Should_Resolve_NewsNoticeCard()
    {
        var card = new WebhookNewsNoticeCardMessage(
            cardImage: new WebhookTemplateCardImage("https://example.com/pic.png"),
            action: WebhookTemplateCardAction.Link("https://example.com"),
            mainTitle: new WebhookTemplateCardMainTitle("图文卡片", null));

        var data = WriteChannelData(new WeChatWorkWebhookTemplateCardChannelData { TemplateCard = card });

        var payload = ChannelDataResolver.Resolve<WeChatWorkWebhookTemplateCardChannelData>(data);

        var restored = payload!.TemplateCard.ShouldBeOfType<WebhookNewsNoticeCardMessage>();
        restored.CardType.ShouldBe("news_notice");
        restored.CardImage.Url.ShouldBe("https://example.com/pic.png");
    }

    [Fact]
    public void Should_Serialize_Card_With_CardType_Discriminator()
    {
        var json = Serialize(new WebhookTextNoticeCardMessage(
            WebhookTemplateCardAction.Link("https://example.com"),
            new WebhookTemplateCardMainTitle("标题", "副标题")));

        // 判别符是还原具体类型所必需的
        json.ShouldContain("card_type");
        json.ShouldContain("text_notice");
    }

    // ---------- 3. 渠道隔离 ----------

    [Fact]
    public void Should_Not_Share_Keys_With_App_Message_Channel()
    {
        var data = WriteChannelData(new WeChatWorkWebhookTextChannelData { Content = "hello" });

        // 应用消息渠道不得命中 Webhook 前缀的载荷
        ChannelDataResolver
            .Resolve<LINGYUN.Abp.Notifications.WeChat.Work.WeChatWorkTextCardChannelData>(data)
            .ShouldBeNull();
        ChannelDataResolver
            .IsDeclared(data, typeof(LINGYUN.Abp.Notifications.WeChat.Work.WeChatWorkTextCardChannelData))
            .ShouldBeFalse();
    }

    [Fact]
    public void Should_Use_Its_Own_Provider_Name()
    {
        Provider.ShouldNotBe(
            LINGYUN.Abp.Notifications.WeChat.Work.WeChatWorkNotificationPublishProvider.ProviderName);
        WeChatWorkWebhookNotificationDataKeys.Prefix.ShouldNotBe(
            LINGYUN.Abp.Notifications.WeChat.Work.WeChatWorkNotificationDataKeys.Prefix);
    }

    // ---------- 4. 注册与工厂 ----------

    [Fact]
    public void Should_Register_All_Eight_Payloads()
    {
        var types = ChannelDataResolver.GetPayloadTypes(Provider);

        types.Count.ShouldBe(8);
        types.ShouldContain(typeof(WeChatWorkWebhookTextChannelData));
        types.ShouldContain(typeof(WeChatWorkWebhookMarkdownChannelData));
        types.ShouldContain(typeof(WeChatWorkWebhookMarkdownV2ChannelData));
        types.ShouldContain(typeof(WeChatWorkWebhookImageChannelData));
        types.ShouldContain(typeof(WeChatWorkWebhookNewsChannelData));
        types.ShouldContain(typeof(WeChatWorkWebhookFileChannelData));
        types.ShouldContain(typeof(WeChatWorkWebhookVoiceChannelData));
        types.ShouldContain(typeof(WeChatWorkWebhookTemplateCardChannelData));
    }

    [Fact]
    public void Should_Map_Every_Payload_Kind_To_Its_Own_MsgType()
    {
        var expected = new (NotificationChannelData Payload, string MsgType)[]
        {
            (new WeChatWorkWebhookTextChannelData { Content = "t" }, "text"),
            (new WeChatWorkWebhookMarkdownChannelData { Content = "m" }, "markdown"),
            (new WeChatWorkWebhookMarkdownV2ChannelData { Content = "m2" }, "markdown_v2"),
            (new WeChatWorkWebhookImageChannelData { Image = new WebhookImageMessage("b", "m") }, "image"),
            (new WeChatWorkWebhookNewsChannelData
            {
                News = new WebhookNewsMessage(new List<WebhookArticleMessage>
                {
                    new WebhookArticleMessage("标题", "https://example.com")
                })
            }, "news"),
            (new WeChatWorkWebhookFileChannelData { File = new WebhookFileMessage("M") }, "file"),
            (new WeChatWorkWebhookVoiceChannelData { Voice = new WebhookVoiceMessage("M") }, "voice"),
            (new WeChatWorkWebhookTemplateCardChannelData
            {
                TemplateCard = new WebhookTextNoticeCardMessage(
                    WebhookTemplateCardAction.Link("https://example.com"),
                    new WebhookTemplateCardMainTitle("标题", "副标题"))
            }, "template_card"),
        };

        foreach (var (payload, msgType) in expected)
        {
            BuildMessage(payload).MsgType.ShouldBe(msgType);
        }

        MessageOptions.MessageKinds.Count.ShouldBe(8);
    }

    // ---------- 5. 选项与优先级 ----------

    [Fact]
    public void Should_Enable_MarkdownV2_By_Default()
    {
        ChannelOptions.UseMarkdownV2.ShouldBeTrue();
    }

    [Fact]
    public void Should_Resolve_Payloads_In_Priority_Order()
    {
        var data = WriteChannelData(new WeChatWorkWebhookTemplateCardChannelData
        {
            TemplateCard = new WebhookTextNoticeCardMessage(
                WebhookTemplateCardAction.Link("https://example.com"),
                new WebhookTemplateCardMainTitle("标题", "副标题"))
        });
        MergeChannelData(data, new WeChatWorkWebhookTextChannelData { Content = "text" });

        var payloads = ChannelDataResolver.Resolve(Provider, data);

        payloads.Count.ShouldBe(2);
        // Priority: 文本 100 < 模板卡片 800
        payloads[0].ShouldBeOfType<WeChatWorkWebhookTextChannelData>();
        payloads[1].ShouldBeOfType<WeChatWorkWebhookTemplateCardChannelData>();
    }

    // ---------- 辅助 ----------

    private WeChatWorkWebhookMessage BuildMessage(NotificationChannelData payload)
    {
        return MessageOptions.Create(new WeChatWorkWebhookMessageFactoryContext(
            new NotificationInfo { Name = "Test", ContentType = NotificationContentType.Text },
            new NotificationStandardData("标题", "内容"),
            (WeChatWorkWebhookChannelData)payload,
            ChannelOptions)).Message;
    }
}
