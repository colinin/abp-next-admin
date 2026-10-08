using System;
using System.Net.Http;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models;

public class OpenObserveRequestException : Exception
{
    public HttpResponseMessage HttpResponseMessage { get; }
    public OpenObserveRequestException(HttpResponseMessage responseMessage)
        : base(responseMessage.ReasonPhrase)
    {
        HttpResponseMessage = responseMessage;
    }
}
