using Volo.Abp;
using Volo.Abp.ExceptionHandling;

namespace LINGYUN.Abp.PushPlus;
public class PushPlusRequestException : AbpException, IHasErrorCode
{
    public string Code { get; }

    public PushPlusRequestException(string code, string message)
        : base($"The PushPlush API returns an error: {code} - {message}")
    {
        Code = code;
    }
}
