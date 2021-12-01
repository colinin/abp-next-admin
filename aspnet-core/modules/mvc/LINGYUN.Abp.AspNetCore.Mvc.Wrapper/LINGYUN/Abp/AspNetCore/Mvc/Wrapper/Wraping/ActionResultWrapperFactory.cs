using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Wraping
{
    public class ActionResultWrapperFactory : IActionResultWrapperFactory
    {
        public IActionResultWrapper CreateFor(FilterContext context)
        {
            Check.NotNull(context, nameof(context));

            return context switch
            {
                ResultExecutingContext resultExecutingContext when resultExecutingContext.Result is ObjectResult => new ObjectActionResultWrapper(),
                ResultExecutingContext resultExecutingContext when resultExecutingContext.Result is JsonResult => new JsonActionResultWrapper(),
                ResultExecutingContext resultExecutingContext when resultExecutingContext.Result is EmptyResult => new EmptyActionResultWrapper(),
                PageHandlerExecutedContext pageHandlerExecutedContext when pageHandlerExecutedContext.Result is ObjectResult => new ObjectActionResultWrapper(),
                PageHandlerExecutedContext pageHandlerExecutedContext when pageHandlerExecutedContext.Result is JsonResult => new JsonActionResultWrapper(),
                PageHandlerExecutedContext pageHandlerExecutedContext when pageHandlerExecutedContext.Result is EmptyResult => new EmptyActionResultWrapper(),
                _ => new NullActionResultWrapper(),
            };
        }
    }
}
