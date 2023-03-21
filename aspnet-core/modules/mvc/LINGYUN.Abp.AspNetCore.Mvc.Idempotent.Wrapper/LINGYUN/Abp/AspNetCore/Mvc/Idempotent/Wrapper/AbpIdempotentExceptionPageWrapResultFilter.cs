using LINGYUN.Abp.AspNetCore.Mvc.Wrapper.ExceptionHandling;
using LINGYUN.Abp.Idempotent;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent.Wrapper;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(AbpExceptionPageFilter),
    typeof(AbpExceptionPageWrapResultFilter))]
public class AbpIdempotentExceptionPageWrapResultFilter : AbpExceptionPageWrapResultFilter, ITransientDependency
{
    protected async override Task HandleAndWrapException(PageHandlerExecutedContext context)
    {
        if (context.Exception is IdempotentDeniedException deniedException)
        {
            var options = context.GetRequiredService<IOptions<AbpIdempotentOptions>>().Value;
            if (!context.HttpContext.Items.ContainsKey(options.IdempotentTokenName))
            {
                context.HttpContext.Items.Add(options.IdempotentTokenName, deniedException.IdempotentKey);
            }
        }
        await base.HandleAndWrapException(context);
    }
}
