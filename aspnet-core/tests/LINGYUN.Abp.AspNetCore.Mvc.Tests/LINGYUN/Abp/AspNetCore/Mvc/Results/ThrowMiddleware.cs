using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.Mvc.Results
{
    public class ThrowMiddleware : IMiddleware, ITransientDependency
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.ToString().StartsWith("/use-throw-middleware"))
            {
                throw new BusinessException("Test:1001");
            }
            await next(context);
        }
    }
}
