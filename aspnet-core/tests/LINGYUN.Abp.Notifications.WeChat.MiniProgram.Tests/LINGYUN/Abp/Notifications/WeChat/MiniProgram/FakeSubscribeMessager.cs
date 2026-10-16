using LINGYUN.Abp.WeChat.MiniProgram.Messages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// 本测试项目未全局启用可空上下文, 而本文件需要表达"可空参数"语义
#nullable enable

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram;

/// <summary>
/// 捕获式 <see cref="ISubscribeMessager"/> 假实现
/// </summary>
/// <remarks>
/// 用于在不真正调用微信接口的前提下, 断言渠道最终发出的模板消息内容。
/// </remarks>
public class FakeSubscribeMessager : ISubscribeMessager
{
    /// <summary>
    /// 已发送的模板消息(按顺序)
    /// </summary>
    public List<SubscribeMessage> SentMessages { get; } = new();

    /// <summary>
    /// 已发送的"按模板参数"形式的调用
    /// </summary>
    public List<SentTemplateArgs> SentTemplates { get; } = new();

    public Task SendAsync(SubscribeMessage message, CancellationToken cancellation = default)
    {
        SentMessages.Add(message);
        return Task.CompletedTask;
    }

    public Task SendAsync(
        Guid toUser,
        string templateId,
        string? page = "",
        string? lang = "zh_CN",
        string? state = "formal",
        Dictionary<string, object?>? data = null,
        CancellationToken cancellation = default)
    {
        SentTemplates.Add(new SentTemplateArgs(toUser, templateId, page, lang, state, data));
        return Task.CompletedTask;
    }

    public void Clear()
    {
        SentMessages.Clear();
        SentTemplates.Clear();
    }

    public record SentTemplateArgs(
        Guid ToUser,
        string TemplateId,
        string? Page,
        string? Lang,
        string? State,
        Dictionary<string, object?>? Data);
}
