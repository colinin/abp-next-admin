using System.Threading.Tasks;

namespace LINGYUN.Platform.Messages;
public interface ISmsMessageManager
{
    Task<SmsMessage> SendAsync(SmsMessage message);
}
