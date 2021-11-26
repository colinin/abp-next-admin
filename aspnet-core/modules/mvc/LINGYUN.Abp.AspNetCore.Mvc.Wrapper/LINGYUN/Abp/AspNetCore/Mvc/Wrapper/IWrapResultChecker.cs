using Microsoft.AspNetCore.Mvc.Filters;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper
{
    public interface IWrapResultChecker
    {
        bool WrapOnExecution(FilterContext context);

        bool WrapOnException(ExceptionContext context);

        bool WrapOnException(PageHandlerExecutedContext context);
    }
}
