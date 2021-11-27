using System;
using Volo.Abp.Http;

namespace LINGYUN.Abp.Wrapper
{
    public class ExceptionWrapContext
    {
        public Exception Exception { get; }
        public IServiceProvider ServiceProvider { get; }
        public RemoteServiceErrorInfo ErrorInfo { get; }
        public ExceptionWrapContext(
            Exception exception,
            RemoteServiceErrorInfo errorInfo,
            IServiceProvider serviceProvider)
        {
            Exception = exception;
            ErrorInfo = errorInfo;
            ServiceProvider = serviceProvider;
        }

        public ExceptionWrapContext WithCode(string code)
        {
            ErrorInfo.Code = code;
            return this;
        }

        public ExceptionWrapContext WithMessage(string message)
        {
            ErrorInfo.Message = message;
            return this;
        }

        public ExceptionWrapContext WithDetails(string details)
        {
            ErrorInfo.Details = details;
            return this;
        }

        public ExceptionWrapContext WithData(string key, object value)
        {
            ErrorInfo.Data[key] = value;
            return this;
        }
    }
}
