using LINGYUN.Abp.WxPusher.Messages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.WxPusher;

/// <summary>
/// 捕获式 <see cref="IWxPusherMessageSender"/> 假实现
/// </summary>
public class FakeWxPusherMessageSender : IWxPusherMessageSender
{
    public List<SentMessageArgs> SentMessages { get; } = new();

    public Task<List<SendMessageResult>> SendAsync(
        string content,
        string? summary = null,
        MessageContentType contentType = MessageContentType.Text,
        List<int>? topicIds = null,
        List<string>? uids = null,
        string? url = null,
        CancellationToken cancellationToken = default)
    {
        SentMessages.Add(new SentMessageArgs(content, summary, contentType, topicIds, uids, url));

        return Task.FromResult(new List<SendMessageResult>());
    }

    public void Clear() => SentMessages.Clear();

    public record SentMessageArgs(
        string Content,
        string? Summary,
        MessageContentType ContentType,
        List<int>? TopicIds,
        List<string>? Uids,
        string? Url);
}
