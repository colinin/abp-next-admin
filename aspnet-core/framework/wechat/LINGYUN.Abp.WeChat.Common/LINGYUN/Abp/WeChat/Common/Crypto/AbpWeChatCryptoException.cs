using LINGYUN.Abp.WeChat.Common;
using System;
using System.Runtime.Serialization;

namespace LINGYUN.Abp.WeChat.Common.Crypto;
public class AbpWeChatCryptoException : AbpWeChatException
{
    public AbpWeChatCryptoException()
    {
    }

    public AbpWeChatCryptoException(
        SerializationInfo serializationInfo,
        StreamingContext context) : base(serializationInfo, context)
    {
    }

    public AbpWeChatCryptoException(
        string appId,
        string message = null,
        string details = null,
        Exception innerException = null)
        : this(appId, "WeChat:100400", message, details, innerException)
    {
    }

    public AbpWeChatCryptoException(
        string appId,
        string code = null,
        string message = null,
        string details = null,
        Exception innerException = null)
        : base(code, message, details, innerException)
    {
        WithData("AppId", appId);
    }
}
