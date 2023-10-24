using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace LINGYUN.Abp.AspNetCore.Wrapper;
public class HttpResponseWrapperContext
{
    public HttpContext HttpContext { get; }
    public int HttpStatusCode { get; }
    public IDictionary<string, string> HttpHeaders { get; }
    public HttpResponseWrapperContext(
        HttpContext httpContext, 
        int httpStatusCode,
        IDictionary<string, string> httpHeaders = null)
    {
        HttpContext = httpContext;
        HttpStatusCode = httpStatusCode;
        HttpHeaders = httpHeaders ?? new Dictionary<string, string>();
    }
}
