using Volo.Abp;
using Volo.Abp.ExceptionHandling;

namespace LINGYUN.Abp.TuiJuhe;

public class TuiJuheRemoteCallException : AbpException, IHasErrorCode
{
    public string Code { get; }

    public TuiJuheRemoteCallException(string code, string message)
        : base($"The TuiJuhe api returns an error: {code} - {message}")
    {
        Code = code;
    }
}
