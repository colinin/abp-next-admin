using System;

namespace LINGYUN.Abp.Wrapper
{
    public interface IExceptionWrapHandlerFactory
    {
        IExceptionWrapHandler CreateFor(ExceptionWrapContext context);
    }
}
