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
            var code = options.CodeWithEmptyResult(context.HttpContext.RequestServices);
            var message = options.MessageWithEmptyResult(context.HttpContext.RequestServices);
            switch (context)
            {
                case ResultExecutingContext resultExecutingContext:
                    resultExecutingContext.Result = new ObjectResult(new WrapResult(code, message));
                    return;

                case PageHandlerExecutedContext pageHandlerExecutedContext:
                    pageHandlerExecutedContext.Result = new ObjectResult(new WrapResult(code, message));
                    return;
            }
        }
    }
}
