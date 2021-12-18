using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Wraping
{
    public class EmptyActionResultWrapper : IActionResultWrapper
    {
        public void Wrap(FilterContext context)
        {
            var options = context.GetRequiredService<IOptions<AbpWrapperOptions>>().Value;
            switch (context)
            {
                case ResultExecutingContext resultExecutingContext:
                    if (options.ErrorWithEmptyResult)
                    {
                        var code = options.CodeWithEmptyResult(context.HttpContext.RequestServices);
                        var message = options.MessageWithEmptyResult(context.HttpContext.RequestServices);
                        resultExecutingContext.Result = new ObjectResult(new WrapResult(code, message));
                        return;
                    }
                    resultExecutingContext.Result = new ObjectResult(new WrapResult(options.CodeWithSuccess, result: null));
                    return;

                case PageHandlerExecutedContext pageHandlerExecutedContext:
                    if (options.ErrorWithEmptyResult)
                    {
                        var code = options.CodeWithEmptyResult(context.HttpContext.RequestServices);
                        var message = options.MessageWithEmptyResult(context.HttpContext.RequestServices);
                        pageHandlerExecutedContext.Result = new ObjectResult(new WrapResult(code, message));
                        return;
                    }
                    pageHandlerExecutedContext.Result = new ObjectResult(new WrapResult(options.CodeWithSuccess, result: null));
                    return;
            }
        }
    }
}
