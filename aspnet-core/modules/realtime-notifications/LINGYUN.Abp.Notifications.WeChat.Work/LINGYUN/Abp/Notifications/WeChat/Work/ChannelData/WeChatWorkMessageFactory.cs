using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications.WeChat.Work;

/// <summary>
/// 企业微信默认消息构造工厂
/// </summary>
/// <remarks>
/// <para>
/// 每一种渠道数据(载荷)与消息载体的映射都是显式的, 新增消息类型只需追加一个
/// <c>Map</c> 调用, 不需要修改 Provider 的发布逻辑。
/// </para>
/// <para>
/// 企业微信各消息载体支持的选项并不一致, 因此按具体载体类型分别重载,
/// 由编译器保证只设置载体真正支持的字段, 而不是像旧实现那样无差别赋值。
/// </para>
/// </remarks>
public static class WeChatWorkMessageFactoryExtensions
{
    /// <summary>
    /// 注册内置的消息构造工厂
    /// </summary>
    public static AbpWeChatWorkNotificationMessageOptions MapDefaultMessageFactories(
        this AbpWeChatWorkNotificationMessageOptions options)
    {
        Check.NotNull(options, nameof(options));

        options.Map(WeChatWorkMessageKind.MiniProgramNotice, context =>
        {
            var channelData = Cast<WeChatWorkMiniProgramNoticeChannelData>(context);
            var miniProgram = channelData.MiniProgram
                ?? throw new AbpNotificationChannelDataConfigurationException(
                    $"The WeChat Work mini program notice of notification '{context.Notification.Name}' is empty.");

            return WeChatWorkMessageFactoryResult.Create(
                Apply(
                    new WeChatWorkMiniProgramMessage(
                        miniProgram,
                        context.Contact.ToUser ?? string.Empty,
                        context.Contact.ToParty ?? string.Empty,
                        context.Contact.ToTag ?? string.Empty),
                    context),
                WeChatWorkMessageKind.MiniProgramNotice);
        });

        options.Map(WeChatWorkMessageKind.MpNews, context =>
        {
            var channelData = Cast<WeChatWorkMpNewChannelData>(context);
            var mpNewMessage = channelData.MpNewMessage
                ?? throw new AbpNotificationChannelDataConfigurationException(
                    $"The WeChat Work mp news of notification '{context.Notification.Name}' is empty.");

            return WeChatWorkMessageFactoryResult.Create(
                Apply(new WeChatWorkMpNewMessage(context.Options.AgentId, mpNewMessage), context),
                WeChatWorkMessageKind.MpNews);
        });

        options.Map(WeChatWorkMessageKind.TemplateCard, context =>
        {
            var channelData = Cast<WeChatWorkTemplateCardChannelData>(context);
            var templateCard = channelData.TemplateCard
                ?? throw new AbpNotificationChannelDataConfigurationException(
                    $"The WeChat Work template card of notification '{context.Notification.Name}' is empty.");

            return WeChatWorkMessageFactoryResult.Create(
                Apply(new WeChatWorkTemplateCardMessage(context.Options.AgentId, templateCard), context),
                WeChatWorkMessageKind.TemplateCard);
        });

        options.Map(WeChatWorkMessageKind.TextCard, context =>
        {
            var channelData = Cast<WeChatWorkTextCardChannelData>(context);
            var textCard = channelData.TextCard
                ?? throw new AbpNotificationChannelDataConfigurationException(
                    $"The WeChat Work text card of notification '{context.Notification.Name}' is empty.");

            return WeChatWorkMessageFactoryResult.Create(
                Apply(new WeChatWorkTextCardMessage(context.Options.AgentId, textCard), context),
                WeChatWorkMessageKind.TextCard);
        });

        options.Map(WeChatWorkMessageKind.Video, context =>
        {
            var channelData = Cast<WeChatWorkVideoChannelData>(context);
            var video = channelData.Video
                ?? throw new AbpNotificationChannelDataConfigurationException(
                    $"The WeChat Work video of notification '{context.Notification.Name}' is empty.");

            return WeChatWorkMessageFactoryResult.Create(
                Apply(new WeChatWorkVideoMessage(context.Options.AgentId, video), context),
                WeChatWorkMessageKind.Video);
        });

        options.Map(WeChatWorkMessageKind.Voice, context =>
        {
            var channelData = Cast<WeChatWorkVoiceChannelData>(context);
            var voice = channelData.Voice
                ?? throw new AbpNotificationChannelDataConfigurationException(
                    $"The WeChat Work voice of notification '{context.Notification.Name}' is empty.");

            return WeChatWorkMessageFactoryResult.Create(
                Apply(new WeChatWorkVoiceMessage(context.Options.AgentId, voice), context),
                WeChatWorkMessageKind.Voice);
        });

        options.Map(WeChatWorkMessageKind.File, context =>
        {
            var channelData = Cast<WeChatWorkFileChannelData>(context);
            var file = channelData.File
                ?? throw new AbpNotificationChannelDataConfigurationException(
                    $"The WeChat Work file of notification '{context.Notification.Name}' is empty.");

            return WeChatWorkMessageFactoryResult.Create(
                Apply(new WeChatWorkFileMessage(context.Options.AgentId, file), context),
                WeChatWorkMessageKind.File);
        });

        options.Map(WeChatWorkMessageKind.Text, context =>
        {
            return WeChatWorkMessageFactoryResult.Create(
                Apply(new WeChatWorkTextMessage(context.Options.AgentId, new TextMessage(GetContent(context))), context),
                WeChatWorkMessageKind.Text);
        });

        options.Map(WeChatWorkMessageKind.Markdown, context =>
        {
            return WeChatWorkMessageFactoryResult.Create(
                Apply(new WeChatWorkMarkdownMessage(context.Options.AgentId, new MarkdownMessage(GetContent(context))), context),
                WeChatWorkMessageKind.Markdown);
        });

        return options;
    }

    private static TChannelData Cast<TChannelData>(WeChatWorkMessageFactoryContext context)
        where TChannelData : WeChatWorkChannelData
    {
        if (context.ChannelData is not TChannelData channelData)
        {
            throw new AbpNotificationChannelDataConfigurationException(
                $"The channel data '{context.ChannelData.GetType().FullName}' of notification '{context.Notification.Name}' does not match the registered factory '{typeof(TChannelData).FullName}'.");
        }

        return channelData;
    }

    private static string GetContent(WeChatWorkMessageFactoryContext context)
    {
        if (context.ChannelData is WeChatWorkTextChannelData textChannelData &&
            !textChannelData.Content.IsNullOrWhiteSpace())
        {
            return textChannelData.Content!;
        }

        if (context.ChannelData is WeChatWorkMarkdownChannelData markdownChannelData &&
            !markdownChannelData.Content.IsNullOrWhiteSpace())
        {
            return markdownChannelData.Content!;
        }

        return context.StandardData.Message;
    }

    /// <summary>
    /// 应用接收者
    /// </summary>
    private static TMessage ApplyContact<TMessage>(TMessage message, WeChatWorkMessageFactoryContext context)
        where TMessage : WeChatWorkMessage
    {
        if (!context.Contact.IsEmpty())
        {
            message.ToUser = context.Contact.ToUser;
            message.ToParty = context.Contact.ToParty;
            message.ToTag = context.Contact.ToTag;
        }

        return message;
    }

    /// <summary>
    /// 文本消息: 支持 safe、enable_id_trans、enable_duplicate_check
    /// </summary>
    private static WeChatWorkTextMessage Apply(
        WeChatWorkTextMessage message,
        WeChatWorkMessageFactoryContext context)
    {
        ApplyContact(message, context);

        message.Safe = context.Options.SafeValue;
        message.EnableIdTrans = context.Options.EnableIdTransValue;
        message.EnableDuplicateCheck = context.Options.EnableDuplicateCheckValue;
        message.DuplicateCheckInterval = context.Options.DuplicateCheckIntervalValue;

        return message;
    }

    /// <summary>
    /// 图文消息: 支持 safe、enable_id_trans、enable_duplicate_check
    /// </summary>
    private static WeChatWorkMpNewMessage Apply(
        WeChatWorkMpNewMessage message,
        WeChatWorkMessageFactoryContext context)
    {
        ApplyContact(message, context);

        message.Safe = context.Options.SafeValue;
        message.EnableIdTrans = context.Options.EnableIdTransValue;
        message.EnableDuplicateCheck = context.Options.EnableDuplicateCheckValue;
        message.DuplicateCheckInterval = context.Options.DuplicateCheckIntervalValue;

        return message;
    }

    /// <summary>
    /// 文件消息: 支持 safe、enable_duplicate_check, 不支持 enable_id_trans
    /// </summary>
    private static WeChatWorkFileMessage Apply(
        WeChatWorkFileMessage message,
        WeChatWorkMessageFactoryContext context)
    {
        ApplyContact(message, context);

        message.Safe = context.Options.SafeValue;
        message.EnableDuplicateCheck = context.Options.EnableDuplicateCheckValue;
        message.DuplicateCheckInterval = context.Options.DuplicateCheckIntervalValue;

        return message;
    }

    /// <summary>
    /// 视频消息: 支持 safe、enable_duplicate_check, 不支持 enable_id_trans
    /// </summary>
    private static WeChatWorkVideoMessage Apply(
        WeChatWorkVideoMessage message,
        WeChatWorkMessageFactoryContext context)
    {
        ApplyContact(message, context);

        message.Safe = context.Options.SafeValue;
        message.EnableDuplicateCheck = context.Options.EnableDuplicateCheckValue;
        message.DuplicateCheckInterval = context.Options.DuplicateCheckIntervalValue;

        return message;
    }

    /// <summary>
    /// 语音消息: 不支持 safe, 也不支持 enable_id_trans
    /// </summary>
    private static WeChatWorkVoiceMessage Apply(
        WeChatWorkVoiceMessage message,
        WeChatWorkMessageFactoryContext context)
    {
        ApplyContact(message, context);

        message.EnableDuplicateCheck = context.Options.EnableDuplicateCheckValue;
        message.DuplicateCheckInterval = context.Options.DuplicateCheckIntervalValue;

        return message;
    }

    /// <summary>
    /// markdown消息: 支持 enable_duplicate_check, 不支持 safe
    /// </summary>
    private static WeChatWorkMarkdownMessage Apply(
        WeChatWorkMarkdownMessage message,
        WeChatWorkMessageFactoryContext context)
    {
        ApplyContact(message, context);

        message.EnableDuplicateCheck = context.Options.EnableDuplicateCheckValue;
        message.DuplicateCheckInterval = context.Options.DuplicateCheckIntervalValue;

        return message;
    }

    /// <summary>
    /// 文本卡片消息: 不支持 safe
    /// </summary>
    private static WeChatWorkTextCardMessage Apply(
        WeChatWorkTextCardMessage message,
        WeChatWorkMessageFactoryContext context)
    {
        ApplyContact(message, context);

        message.EnableIdTrans = context.Options.EnableIdTransValue;
        message.EnableDuplicateCheck = context.Options.EnableDuplicateCheckValue;
        message.DuplicateCheckInterval = context.Options.DuplicateCheckIntervalValue;

        return message;
    }

    /// <summary>
    /// 模板卡片消息: 不支持 safe
    /// </summary>
    private static WeChatWorkTemplateCardMessage Apply(
        WeChatWorkTemplateCardMessage message,
        WeChatWorkMessageFactoryContext context)
    {
        ApplyContact(message, context);

        message.EnableIdTrans = context.Options.EnableIdTransValue;
        message.EnableDuplicateCheck = context.Options.EnableDuplicateCheckValue;
        message.DuplicateCheckInterval = context.Options.DuplicateCheckIntervalValue;

        return message;
    }

    /// <summary>
    /// 小程序通知消息: 不支持 safe
    /// </summary>
    private static WeChatWorkMiniProgramMessage Apply(
        WeChatWorkMiniProgramMessage message,
        WeChatWorkMessageFactoryContext context)
    {
        ApplyContact(message, context);

        message.EnableIdTrans = context.Options.EnableIdTransValue;
        message.EnableDuplicateCheck = context.Options.EnableDuplicateCheckValue;
        message.DuplicateCheckInterval = context.Options.DuplicateCheckIntervalValue;

        return message;
    }
}
