using System;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models.Search;

public class OpenObserveSearchException : Exception
{
    public int Code => Error.Code;
    public OpenObserveErrorResponse Error { get; }
    public OpenObserveSearchException(OpenObserveErrorResponse error)
        : base(error.Message)
    {
        Error = error;
    }
}
