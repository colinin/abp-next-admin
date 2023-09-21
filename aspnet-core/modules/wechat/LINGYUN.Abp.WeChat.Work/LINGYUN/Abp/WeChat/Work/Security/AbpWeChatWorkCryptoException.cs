using System;
using System.Runtime.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Security;
public class AbpWeChatWorkCryptoException : AbpWeChatWorkException
{
    public AbpWeChatWorkCryptoException()
    {
    }

    public AbpWeChatWorkCryptoException(
        SerializationInfo serializationInfo, 
        StreamingContext context) : base(serializationInfo, context)
    {
    }

    public AbpWeChatWorkCryptoException(
        string agentId,
        string message = null, 
        string details = null, 
        Exception innerException = null) 
        : this(agentId, "WeChatWork:100400", message, details, innerException)
    {
    }

    public AbpWeChatWorkCryptoException(
        string agentId,
        string code = null,
        string message = null,
        string details = null,
        Exception innerException = null)
        : base(code, message, details, innerException)
    {
        WithData("AgentId", agentId);
    }
}
