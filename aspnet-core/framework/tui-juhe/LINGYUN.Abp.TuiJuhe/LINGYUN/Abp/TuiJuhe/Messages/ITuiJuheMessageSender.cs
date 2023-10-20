using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.TuiJuhe.Messages;

public interface ITuiJuheMessageSender
{
    Task<TuiJuheResult<object, object>> SendAsync(
        [NotNull] string title,
        [NotNull] string content,
        [NotNull] string serviceId,
        MessageContentType contentType = MessageContentType.Text,
        CancellationToken cancellationToken = default);
}
