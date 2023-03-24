using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.Wrapper;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.Mvc;

public class HttpResponseWrapper : IHttpResponseWrapper, ITransientDependency
{
    protected AbpWrapperOptions Options { get; }

    public HttpResponseWrapper(IOptions<AbpWrapperOptions> options)
    {
        Options = options.Value;
    }

    public virtual void Wrap(HttpResponseWrapperContext context)
    {
        context.HttpContext.Response.StatusCode = context.HttpStatusCode;
        if (context.HttpHeaders != null)
        {
            foreach (var header in context.HttpHeaders)
            {
                if (!context.HttpContext.Response.Headers.ContainsKey(header.Key))
                {
                    context.HttpContext.Response.Headers.Add(header.Key, header.Value);
                }
            }
        }
        if (!context.HttpContext.Response.Headers.ContainsKey(AbpHttpWrapConsts.AbpWrapResult))
        {
            context.HttpContext.Response.Headers.Add(AbpHttpWrapConsts.AbpWrapResult, "true");
        }
    }
}
