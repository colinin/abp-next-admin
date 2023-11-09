using System;
using System.Runtime.Serialization;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.Common;

public class AbpWeChatException : BusinessException
{
    public AbpWeChatException()
    {
    }

    public AbpWeChatException(
        string code = null,
        string message = null,
        string details = null,
        Exception innerException = null)
        : base(code, message, details, innerException)
    {
    }

    public AbpWeChatException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }
}
