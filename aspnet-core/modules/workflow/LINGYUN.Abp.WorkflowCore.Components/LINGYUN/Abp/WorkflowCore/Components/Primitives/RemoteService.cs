using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Components.Primitives
{
    public class RemoteService : StepBodyAsyncBase
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IServiceProvider _serviceProvider;
        public RemoteService(
            ICurrentTenant currentTenant,
            IServiceProvider serviceProvider)
        {
            _currentTenant = currentTenant;
            _serviceProvider = serviceProvider;

            Data = new Dictionary<string, object>();
        }
        /// <summary>
        /// 远程服务接口类型
        /// </summary>
        public string Interface { get; set; }
        /// <summary>
        /// 远程服务方法名称
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public Dictionary<string, object> Data { get; set; }
        /// <summary>
        /// 调用结果
        /// </summary>
        public object Result { get; set; }

        public Guid? TenantId { get; set; }
        public string CurrentCulture { get; set; }
        
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var serviceType = Type.GetType(Interface, true, true);
            var method = serviceType.GetMethod(Method);

            var serviceFactory = _serviceProvider.GetRequiredService(serviceType);

            using (_currentTenant.Change(TenantId))
            {
                using (CultureHelper.Use(CurrentCulture ?? CultureInfo.CurrentCulture.Name))
                {
                    // TODO: 身份令牌? 
                    // 工作流中是否需要调用API, 还是用户调用API之后传递事件激活下一个步骤

                    // Abp Api动态代理
                    var result = (Task)method.Invoke(serviceFactory, Data.Select(x => x.Value).ToArray());
                    await result;

                    if (!method.ReturnType.GenericTypeArguments.IsNullOrEmpty())
                    {
                        var resultType = method.ReturnType.GenericTypeArguments[0];
                        var resultProperty = typeof(Task<>)
                            .MakeGenericType(resultType)
                            .GetProperty(nameof(Task<object>.Result), BindingFlags.Instance | BindingFlags.Public);

                        Result = resultProperty.GetValue(result);
                    }

                    return ExecutionResult.Next();
                }
            }
        }
    }
}
