using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper
{
    public class WrapResultChecker : IWrapResultChecker, ISingletonDependency
    {
        protected AbpWrapperOptions Options { get; }

        public WrapResultChecker(IOptionsMonitor<AbpWrapperOptions> optionsMonitor)
        {
            Options = optionsMonitor.CurrentValue;
        }

        public bool WrapOnException(ExceptionContext context)
        {
            if (!CheckForBase(context))
            {
                return false;
            }

            return CheckForException(context.Exception);
        }

        public bool WrapOnException(PageHandlerExecutedContext context)
        {
            return CheckForException(context.Exception);
        }

        public bool WrapOnExecution(FilterContext context)
        {
            return CheckForBase(context);
        }


        protected virtual bool CheckForBase(FilterContext context)
        {
            if (!Options.IsEnabled)
            {
                return false;
            }

            if (context.HttpContext.Request.Headers.ContainsKey(AbpHttpWrapConsts.AbpDontWrapResult))
            {
                return false;
            }

            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                if (!context.ActionDescriptor.HasObjectResult())
                {
                    return false;
                }

                //if (!context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
                //{
                //    return false;
                //}

                if (!CheckForUrl(context))
                {
                    return false;
                }

                if (!CheckForNamespace(descriptor))
                {
                    return false;
                }

                if (!CheckForController(descriptor))
                {
                    return false;
                }

                if (!CheckForInterfaces(descriptor))
                {
                    return false;
                }

                if (!CheckForMethod(descriptor))
                {
                    return false;
                }

                if (!CheckForReturnType(descriptor))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        protected virtual bool CheckForUrl(FilterContext context)
        {
            if (!Options.IgnorePrefixUrls.Any())
            {
                return true;
            }
            var url = BuildUrl(context.HttpContext);
            return !Options.IgnorePrefixUrls.Any(urlPrefix => urlPrefix.StartsWith(url));
        }

        protected virtual bool CheckForController(ControllerActionDescriptor controllerActionDescriptor)
        {
            if (controllerActionDescriptor.ControllerTypeInfo.IsDefined(typeof(IgnoreWrapResultAttribute), true))
            {
                return false;
            }

            return !Options.IgnoreControllers.Any(controller =>
                controller.IsAssignableFrom(controllerActionDescriptor.ControllerTypeInfo));
        }

        protected virtual bool CheckForMethod(ControllerActionDescriptor controllerActionDescriptor)
        {
            return !controllerActionDescriptor.MethodInfo.IsDefined(typeof(IgnoreWrapResultAttribute), true);
        }

        protected virtual bool CheckForNamespace(ControllerActionDescriptor controllerActionDescriptor)
        {
            if (string.IsNullOrWhiteSpace(controllerActionDescriptor.ControllerTypeInfo.Namespace))
            {
                return true;
            }

            return !Options.IgnoreNamespaces.Any(nsp =>
                controllerActionDescriptor.ControllerTypeInfo.Namespace.StartsWith(nsp));
        }

        protected virtual bool CheckForInterfaces(ControllerActionDescriptor controllerActionDescriptor)
        {
            return !Options.IgnoredInterfaces.Any(type =>
                type.IsAssignableFrom(controllerActionDescriptor.ControllerTypeInfo));
        }

        protected virtual bool CheckForReturnType(ControllerActionDescriptor controllerActionDescriptor)
        {
            var returnType = AsyncHelper.UnwrapTask(controllerActionDescriptor.MethodInfo.ReturnType);

            if (returnType.IsDefined(typeof(IgnoreWrapResultAttribute), true))
            {
                return false;
            }

            return !Options.IgnoreReturnTypes.Any(type => returnType.IsAssignableFrom(type));
        }

        protected virtual bool CheckForException(Exception exception)
        {
            return !Options.IgnoreExceptions.Any(ex => ex.IsAssignableFrom(exception.GetType()));
        }

        protected virtual string BuildUrl(HttpContext httpContext)
        {
            //TODO: Add options to include/exclude query, schema and host

            var uriBuilder = new UriBuilder();

            uriBuilder.Scheme = httpContext.Request.Scheme;
            uriBuilder.Host = httpContext.Request.Host.Host;
            uriBuilder.Path = httpContext.Request.Path.ToString();
            uriBuilder.Query = httpContext.Request.QueryString.ToString();

            return uriBuilder.Uri.AbsolutePath;
        }
    }
}
