using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.Wraping
{
    public class JsonActionResultWrapper : IActionResultWrapper
    {
        public void Wrap(FilterContext context)
        {
            JsonResult jsonResult = null;

            switch (context)
            {
                case ResultExecutingContext resultExecutingContext:
                    jsonResult = resultExecutingContext.Result as JsonResult;
                    break;

                case PageHandlerExecutedContext pageHandlerExecutedContext:
                    jsonResult = pageHandlerExecutedContext.Result as JsonResult;
                    break;
            }

            if (jsonResult == null)
            {
                throw new ArgumentException("Action Result should be JsonResult!");
            }

            if (!(jsonResult.Value is WrapResult))
            {
                var options = context.GetRequiredService<IOptions<AbpWrapperOptions>>().Value;

                jsonResult.Value = new WrapResult(options.CodeWithSuccess, jsonResult.Value);
            }
        }
    }
}
