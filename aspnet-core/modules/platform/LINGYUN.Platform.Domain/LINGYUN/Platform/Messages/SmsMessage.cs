using System;

namespace LINGYUN.Platform.Messages;
public class SmsMessage : Message
{
    protected SmsMessage()
    {
    }
    public SmsMessage(
        Guid id,
        string phoneNumber,
        string content,
        Guid? userId = null,
        string userName = null)
        : base(id, phoneNumber, content, userId, userName)
    {
    }
}
