using System;
using System.Collections.Generic;
using System.Net;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.Wrapper
{
    public class AbpWrapperOptions
    {
        /// <summary>
        /// 未处理异常代码
        /// 默认: 500
        /// </summary>
        public string CodeWithUnhandled { get; set; }
        /// <summary>
        /// 是否启用包装器
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 成功时返回代码
        /// 默认：0
        /// </summary>
        public string CodeWithSuccess { get; set; }
        /// <summary>
        /// 资源为空时是否提示错误
        /// 默认: false
        /// </summary>
        public bool ErrorWithEmptyResult { get; set; }
        /// <summary>
        /// 资源为空时返回代码
        /// 默认：404
        /// </summary>
        public Func<IServiceProvider, string> CodeWithEmptyResult { get; set; }
        /// <summary>
        /// 资源为空时返回错误消息
        /// </summary>
        public Func<IServiceProvider, string> MessageWithEmptyResult { get; set; }
        /// <summary>
        /// 包装后的返回状态码
        /// 默认：200  HttpStatusCode.OK
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }
        /// <summary>
        /// 忽略Url开头类型
        /// </summary>
        public IList<string> IgnorePrefixUrls { get; }
        /// <summary>
        /// 忽略指定命名空间
        /// </summary>
        public IList<string> IgnoreNamespaces { get; }
        /// <summary>
        /// 忽略控制器
        /// </summary>
        public ITypeList IgnoreControllers { get; }
        /// <summary>
        /// 忽略返回值
        /// </summary>
        public ITypeList IgnoreReturnTypes { get; }
        /// <summary>
        /// 忽略异常
        /// </summary>
        public ITypeList<Exception> IgnoreExceptions { get; }
        /// <summary>
        /// 忽略接口类型
        /// </summary>
        public ITypeList IgnoredInterfaces { get; }

        internal IDictionary<Type, IExceptionWrapHandler> ExceptionHandles { get; }

        public AbpWrapperOptions()
        {
            CodeWithUnhandled = "500";
            CodeWithSuccess = "0";
            HttpStatusCode = HttpStatusCode.OK;
            ErrorWithEmptyResult = false;

            IgnorePrefixUrls = new List<string>();
            IgnoreNamespaces = new List<string>();

            IgnoreControllers = new TypeList();
            IgnoreReturnTypes = new TypeList();
            IgnoredInterfaces = new TypeList()
            {
                typeof(IWrapDisabled)
            };
            IgnoreExceptions = new TypeList<Exception>();

            CodeWithEmptyResult = (_) => "404";
            MessageWithEmptyResult = (_) => "Not Found";

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
