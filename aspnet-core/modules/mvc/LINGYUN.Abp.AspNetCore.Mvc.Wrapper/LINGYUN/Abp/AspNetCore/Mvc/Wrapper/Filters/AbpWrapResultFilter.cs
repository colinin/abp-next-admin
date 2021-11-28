using LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Wraping;
using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Filters
{
    public class AbpWrapResultFilter : IAsyncResultFilter, ITransientDependency
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (ShouldWrapResult(context))
            {
                await HandleAndWrapResult(context);
            }

            await next();
        }

        protected virtual bool ShouldWrapResult(ResultExecutingContext context)
        {
            var wrapResultChecker = context.GetRequiredService<IWrapResultChecker>();

            return wrapResultChecker.WrapOnExecution(context);
        }

        protected virtual Task HandleAndWrapResult(ResultExecutingContext context)
        {
            var actionResultWrapperFactory = context.GetRequiredService<IActionResultWrapperFactory>();
            actionResultWrapperFactory.CreateFor(context).Wrap(context);
            context.HttpContext.Response.Headers.Add(AbpHttpWrapConsts.AbpWrapResult, "true");

            return Task.CompletedTask;
        }
    }
}
