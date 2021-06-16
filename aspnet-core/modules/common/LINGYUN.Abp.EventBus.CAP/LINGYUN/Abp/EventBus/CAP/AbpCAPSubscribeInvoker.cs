using DotNetCore.CAP;
using DotNetCore.CAP.Filter;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP.Serialization;
using LINGYUN.Abp.EventBus.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// 重写 ISubscribeInvoker 实现 Abp 租户集成
    /// </summary>
    public class AbpCAPSubscribeInvoker : ISubscribeInvoker
    {
        private readonly ICurrentTenant _currentTenant;

        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISerializer _serializer;
        private readonly ConcurrentDictionary<string, ObjectMethodExecutor> _executors;
        /// <summary>
        /// AbpCAPSubscribeInvoker
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="serializer"></param>
        /// <param name="currentTenant"></param>
        public AbpCAPSubscribeInvoker(
            ILoggerFactory loggerFactory, 
            IServiceProvider serviceProvider, 
            ISerializer serializer,
            ICurrentTenant currentTenant) 
        {
            _currentTenant = currentTenant;
            _serviceProvider = serviceProvider;
            _serializer = serializer;
            _logger = loggerFactory.CreateLogger<SubscribeInvoker>();
            _executors = new ConcurrentDictionary<string, ObjectMethodExecutor>();
        }
        /// <summary>
        /// 调用订阅者方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<ConsumerExecutedResult> InvokeAsync(ConsumerContext context, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var methodInfo = context.ConsumerDescriptor.MethodInfo;
            var reflectedType = methodInfo.ReflectedType.Name;

            _logger.LogDebug("Executing subscriber method : {0}", methodInfo.Name);

            var key = $"{methodInfo.Module.Name}_{reflectedType}_{methodInfo.MetadataToken}";
            var executor = _executors.GetOrAdd(key, x => ObjectMethodExecutor.Create(methodInfo, context.ConsumerDescriptor.ImplTypeInfo));

            using var scope = _serviceProvider.CreateScope();

            var provider = scope.ServiceProvider;

            var obj = GetInstance(provider, context);

            var message = context.DeliverMessage;
            var parameterDescriptors = context.ConsumerDescriptor.Parameters;
            var executeParameters = new object[parameterDescriptors.Count];
            // 租户数据可能在消息标头中
            var tenantId = message.GetTenantIdOrNull();
            for (var i = 0; i < parameterDescriptors.Count; i++)
            {
                if (parameterDescriptors[i].IsFromCap)
                {
                    executeParameters[i] = new CapHeader(message.Headers);
                }
                else
                {
                    if (message.Value != null)
                    {
                        if (_serializer.IsJsonType(message.Value))  // use ISerializer when reading from storage, skip other objects if not Json
                        {
                            var eventData = _serializer.Deserialize(message.Value, parameterDescriptors[i].ParameterType);
                            // 租户数据也可能存在事件数据中
                            if (tenantId == null && eventData is IMultiTenant tenant)
                            {
                                tenantId = tenant.TenantId;
                            }
                            executeParameters[i] = eventData;
                        }
                        else
                        {
                            var converter = TypeDescriptor.GetConverter(parameterDescriptors[i].ParameterType);
                            if (converter.CanConvertFrom(message.Value.GetType()))
                            {
                                var eventData = converter.ConvertFrom(message.Value);
                                // 租户数据也可能存在事件数据中
                                if (tenantId == null && eventData is IMultiTenant tenant)
                                {
                                    tenantId = tenant.TenantId;
                                }
                                executeParameters[i] = eventData;
                            }
                            else
                            {
                                if (parameterDescriptors[i].ParameterType.IsInstanceOfType(message.Value))
                                {
                                    // 租户数据也可能存在事件数据中
                                    if (tenantId == null && message.Value is IMultiTenant tenant)
                                    {
                                        tenantId = tenant.TenantId;
                                    }
                                    executeParameters[i] = message.Value;
                                }
                                else
                                {
                                    var eventData = Convert.ChangeType(message.Value, parameterDescriptors[i].ParameterType);
                                    // 租户数据也可能存在事件数据中
                                    if (tenantId == null && eventData is IMultiTenant tenant)
                                    {
                                        tenantId = tenant.TenantId;
                                    }
                                    executeParameters[i] = eventData;
                                }
                            }
                        }
                    }
                }
            }

            // 改变租户
            using (_currentTenant.Change(tenantId))
            {
                var filter = provider.GetService<ISubscribeFilter>();
                object resultObj = null;

                try
                {
                    if (filter != null)
                    {
                        var etContext = new ExecutingContext(context, executeParameters);
                        filter.OnSubscribeExecuting(etContext);
                        executeParameters = etContext.Arguments;
                    }

                    resultObj = await ExecuteWithParameterAsync(executor, obj, executeParameters);

                    if (filter != null)
                    {
                        var edContext = new ExecutedContext(context, resultObj);
                        filter.OnSubscribeExecuted(edContext);
                        resultObj = edContext.Result;
                    }
                }
                catch (Exception e)
                {
                    if (filter != null)
                    {
                        var exContext = new ExceptionContext(context, e);
                        filter.OnSubscribeException(exContext);
                        if (!exContext.ExceptionHandled)
                        {
                            throw exContext.Exception;
                        }

                        if (exContext.Result != null)
                        {
                            resultObj = exContext.Result;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }

                return new ConsumerExecutedResult(resultObj, message.GetId(), message.GetCallbackName());
            }
        }
        /// <summary>
        /// 获取事件处理类实例
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual object GetInstance(IServiceProvider provider, ConsumerContext context)
        {
            var srvType = context.ConsumerDescriptor.ServiceTypeInfo?.AsType();
            var implType = context.ConsumerDescriptor.ImplTypeInfo.AsType();

            object obj = null;
            if (srvType != null)
            {
                obj = provider.GetServices(srvType).FirstOrDefault(o => o.GetType() == implType);
            }

            if (obj == null)
            {
                obj = ActivatorUtilities.GetServiceOrCreateInstance(provider, implType);
            }

            return obj;
        }
        /// <summary>
        /// 通过给定的类型实例与参数调用订阅者方法
        /// </summary>
        /// <param name="executor"></param>
        /// <param name="class"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private async Task<object> ExecuteWithParameterAsync(ObjectMethodExecutor executor, object @class, object[] parameter)
        {
            if (executor.IsMethodAsync)
            {
                return await executor.ExecuteAsync(@class, parameter);
            }

            return executor.Execute(@class, parameter);
        }
    }
}
