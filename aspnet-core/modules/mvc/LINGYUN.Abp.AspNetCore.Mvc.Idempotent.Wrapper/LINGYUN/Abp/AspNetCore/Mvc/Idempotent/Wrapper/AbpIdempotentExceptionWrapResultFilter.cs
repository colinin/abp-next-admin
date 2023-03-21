using LINGYUN.Abp.AspNetCore.Mvc.Wrapper.ExceptionHandling;
using LINGYUN.Abp.Idempotent;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(AbpExceptionFilter),
    typeof(AbpExceptionWrapResultFilter))]
public class AbpIdempotentExceptionWrapResultFilter : AbpExceptionWrapResultFilter, ITransientDependency
{
    protected async override Task HandleAndWrapException(ExceptionContext context)
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
