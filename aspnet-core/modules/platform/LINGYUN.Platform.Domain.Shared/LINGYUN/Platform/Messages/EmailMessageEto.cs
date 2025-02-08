using Volo.Abp.EventBus;

namespace LINGYUN.Platform.Messages;

[EventName("platform.messages.email")]
public class EmailMessageEto : MessageEto
{
    public string From { get; set; }
    public string Subject { get; set; }
}
