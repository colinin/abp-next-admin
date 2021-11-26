using System;
using System.Collections.Generic;
using System.Net;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper
{
    public class AbpAspNetCoreMvcWrapperOptions
    {
        /// <summary>
        /// 是否启用包装器
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 资源有效时返回代码
        /// 默认：0
        /// </summary>
        public string CodeWithFound { get; set; }
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

        public AbpAspNetCoreMvcWrapperOptions()
        {
            CodeWithFound = "0";
            HttpStatusCode = HttpStatusCode.OK;

            IgnorePrefixUrls = new List<string>();
            IgnoreNamespaces = new List<string>();

            IgnoreControllers = new TypeList();
            IgnoreReturnTypes = new TypeList();
            IgnoreExceptions = new TypeList<Exception>();

            CodeWithEmptyResult = (_) => "404";
            MessageWithEmptyResult = (_) => "Not Found";
        }
    }
}
