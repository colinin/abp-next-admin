using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace LINGYUN.Abp.AspNetCore.Mvc.Wrapper.ExceptionHandling
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(AbpExceptionPageFilter))]
    public class AbpExceptionPageWrapResultFilter: AbpExceptionPageFilter, ITransientDependency
    {
        protected override async Task HandleAndWrapException(PageHandlerExecutedContext context)
        {
            var wrapResultChecker = context.GetRequiredService<IWrapResultChecker>();
            if (!wrapResultChecker.WrapOnException(context))
            {
                await base.HandleAndWrapException(context);
                return;
            }

            var wrapOptions = context.GetRequiredService<IOptions<AbpWrapperOptions>>().Value;
            var exceptionHandlingOptions = context.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;
            var exceptionToErrorInfoConverter = context.GetRequiredService<IExceptionToErrorInfoConverter>();
            var remoteServiceErrorInfo = exceptionToErrorInfoConverter.Convert(context.Exception, options =>
            {
                options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
                options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
            });

            var logLevel = context.Exception.GetLogLevel();

            var remoteServiceErrorInfoBuilder = new StringBuilder();
            remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
            remoteServiceErrorInfoBuilder.AppendLine(context.GetRequiredService<IJsonSerializer>().Serialize(remoteServiceErrorInfo, indented: true));

            var logger = context.GetService<ILogger<AbpExceptionPageWrapResultFilter>>(NullLogger<AbpExceptionPageWrapResultFilter>.Instance);
            logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());

            logger.LogException(context.Exception, logLevel);

            await context.GetRequiredService<IExceptionNotifier>().NotifyAsync(new ExceptionNotificationContext(context.Exception));


            if (context.Exception is AbpAuthorizationException)
            {
                await context.HttpContext.RequestServices.GetRequiredService<IAbpAuthorizationExceptionHandler>()
                    .HandleAsync(context.Exception.As<AbpAuthorizationException>(), context.HttpContext);
            }
            else
            {
                var statusCodFinder = context.GetRequiredService<IHttpExceptionStatusCodeFinder>();
                var exceptionWrapHandler = context.GetRequiredService<IExceptionWrapHandlerFactory>();
                var exceptionWrapContext = new ExceptionWrapContext(
                    context.Exception,
                    remoteServiceErrorInfo,
                    context.HttpContext.RequestServices,
                    statusCodFinder.GetStatusCode(context.HttpContext, context.Exception));
                exceptionWrapHandler.CreateFor(exceptionWrapContext).Wrap(exceptionWrapContext);
                context.Result = new ObjectResult(new WrapResult(
                    exceptionWrapContext.ErrorInfo.Code,
                    exceptionWrapContext.ErrorInfo.Message,
                    exceptionWrapContext.ErrorInfo.Details));

                context.HttpContext.Response.Headers.Add(AbpHttpWrapConsts.AbpWrapResult, "true");
                context.HttpContext.Response.StatusCode = (int)wrapOptions.HttpStatusCode;
            }

            context.Exception = null; //Handled!
        }
    }
}
