using Volo.Abp;
using Volo.Abp.ExceptionHandling;

namespace LINGYUN.Abp.WxPusher;

public class WxPusherRemoteCallException : AbpException, IHasErrorCode
{
    public string Code { get; }

    public WxPusherRemoteCallException(string code, string message)
        : base($"The WxPusher api returns an error: {code} - {message}")
    {
        Code = code;
    }
}
