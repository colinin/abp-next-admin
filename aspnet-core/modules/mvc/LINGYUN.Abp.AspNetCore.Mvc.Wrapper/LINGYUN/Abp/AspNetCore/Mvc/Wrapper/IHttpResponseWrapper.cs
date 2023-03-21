using Microsoft.AspNetCore.Http;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
public interface IHttpResponseWrapper
{
    void Wrap(HttpResponseWrapperContext context);
}
