using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.ExceptionHandling;

namespace LINGYUN.Abp.Wrapper
{
    public class DefaultExceptionWrapHandler : IExceptionWrapHandler
    {
        public void Wrap(ExceptionWrapContext context)
        {
            if (context.Exception is IHasErrorCode exceptionWithErrorCode)
            {
                string errorCode;
                if (!exceptionWithErrorCode.Code.IsNullOrWhiteSpace() &&
                    exceptionWithErrorCode.Code.Contains(":"))
                {
                    errorCode = exceptionWithErrorCode.Code.Split(':')[1];
                }
                else
                {
                    errorCode = exceptionWithErrorCode.Code;
                }

                context.WithCode(errorCode);
            }

            // 没有处理的异常代码统一用配置代码处理
            if (context.ErrorInfo.Code.IsNullOrWhiteSpace())
            {
                if (context.StatusCode.HasValue)
                {
                    context.WithCode(((int)context.StatusCode).ToString());
                    return;
                }
                var wrapperOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpWrapperOptions>>().Value;
                context.WithCode(wrapperOptions.CodeWithUnhandled);
            }
        }
    }
}
