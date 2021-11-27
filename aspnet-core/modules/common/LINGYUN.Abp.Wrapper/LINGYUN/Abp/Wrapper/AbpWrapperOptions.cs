using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Wrapper
{
    public class AbpWrapperOptions
    {
        /// <summary>
        /// 未处理异常代码
        /// 默认: 500
        /// </summary>
        public string CodeWithUnhandled { get; set; }

        internal IDictionary<Type, IExceptionWrapHandler> ExceptionHandles { get; }

        public AbpWrapperOptions()
        {
            CodeWithUnhandled = "500";
            ExceptionHandles = new Dictionary<Type, IExceptionWrapHandler>();
        }

        public void AddHandler<TException>(IExceptionWrapHandler handler)
            where TException : Exception
        {
            AddHandler(typeof(TException), handler);
        }

        public void AddHandler(Type exceptionType, IExceptionWrapHandler handler)
        {
            ExceptionHandles[exceptionType] = handler;
        }

        public IExceptionWrapHandler GetHandler(Type exceptionType)
        {
            ExceptionHandles.TryGetValue(exceptionType, out IExceptionWrapHandler handler);

            return handler;
        }
    }
}
