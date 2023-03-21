using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.Idempotent;
using LINGYUN.Abp.Wrapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent.Wrapper;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IHttpResponseWrapper),
    typeof(HttpResponseWrapper))]
public class IdempotentHttpResponseWrapper : HttpResponseWrapper, ITransientDependency
{
    protected AbpIdempotentOptions IdempotentOptions { get; }
    public IdempotentHttpResponseWrapper(
        IOptions<AbpWrapperOptions> wrapperOptions,
        IOptions<AbpIdempotentOptions> idempotentOptions) : base(wrapperOptions)
    {
        IdempotentOptions = idempotentOptions.Value;
    }

    public override void Wrap(HttpResponseWrapperContext context)
    {
        if (context.HttpContext.Items.TryGetValue(nameof(IdempotentAttribute.RedirectUrl), out var redirectUrl) && redirectUrl != null)
        {
            context.HttpContext.Response.Headers.Add(AbpHttpWrapConsts.AbpWrapResult, "true");
            context.HttpContext.Response.Redirect(redirectUrl.ToString());
            return;
        }

        if (context.HttpContext.Items.TryGetValue(IdempotentOptions.IdempotentTokenName, out var idempotentKey) && idempotentKey != null)
        {
            context.HttpHeaders.TryAdd(IdempotentOptions.IdempotentTokenName, idempotentKey.ToString());
        }

        base.Wrap(context);
    }
}
