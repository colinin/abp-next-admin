using System.Threading.Tasks;

namespace LINGYUN.Platform.Messages;
public interface IEmailMessageManager
{
    Task<EmailMessage> SendAsync(EmailMessage message);
}
