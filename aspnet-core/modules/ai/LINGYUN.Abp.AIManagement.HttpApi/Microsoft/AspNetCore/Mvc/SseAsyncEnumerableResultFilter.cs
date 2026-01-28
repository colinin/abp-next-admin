using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc;
public class SseAsyncEnumerableResultFilter : IAsyncActionFilter
{
    public async  Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executedContext = await next();

        if (executedContext.Result is ObjectResult objectResult &&
            objectResult.Value is IAsyncEnumerable<string> asyncEnumerable)
        {
            executedContext.Result = new SseAsyncEnumerableResult(asyncEnumerable);
        }
    }
}
