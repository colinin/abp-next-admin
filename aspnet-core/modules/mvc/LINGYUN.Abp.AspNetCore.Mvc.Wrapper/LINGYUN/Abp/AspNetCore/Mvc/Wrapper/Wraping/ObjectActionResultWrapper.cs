using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Wraping
{
    public class ObjectActionResultWrapper : IActionResultWrapper
    {
        public void Wrap(FilterContext context)
        {
            ObjectResult objectResult = null;

            switch (context)
            {
                case ResultExecutingContext resultExecutingContext:
                    objectResult = resultExecutingContext.Result as ObjectResult;
                    break;

                case PageHandlerExecutedContext pageHandlerExecutedContext:
                    objectResult = pageHandlerExecutedContext.Result as ObjectResult;
                    break;
            }

            if (objectResult == null)
            {
                throw new ArgumentException("Action Result should be ObjectResult!");
            }

            if (!(objectResult.Value is WrapResult))
            {
                var options = context.GetRequiredService<IOptions<AbpWrapperOptions>>().Value;

                if (objectResult.Value == null && options.ErrorWithEmptyResult)
                {
                    var code = options.CodeWithEmptyResult(context.HttpContext.RequestServices);
                    var message = options.MessageWithEmptyResult(context.HttpContext.RequestServices);
                    objectResult.Value = new WrapResult(code, message);
                }
                else
                {
                    objectResult.Value = new WrapResult(options.CodeWithSuccess, objectResult.Value);
                }

                objectResult.DeclaredType = typeof(WrapResult);
            }
        }
    }
}
