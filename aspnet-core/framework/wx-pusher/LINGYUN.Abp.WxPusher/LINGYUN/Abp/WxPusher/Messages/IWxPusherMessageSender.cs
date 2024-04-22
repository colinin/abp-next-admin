using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.Messages;

public interface IWxPusherMessageSender
{
    Task<List<SendMessageResult>> SendAsync(
        string content,
        string summary = "",
        MessageContentType contentType = MessageContentType.Text,
        List<int> topicIds = null,
        List<string> uids = null,
        string url = "",
        CancellationToken cancellationToken = default);
}
