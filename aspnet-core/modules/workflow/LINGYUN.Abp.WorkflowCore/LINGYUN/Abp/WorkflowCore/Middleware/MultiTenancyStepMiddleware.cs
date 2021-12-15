using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Middleware
{
    public class MultiTenancyStepMiddleware : IWorkflowStepMiddleware
    {
        private readonly AbpMultiTenancyOptions _multiTenancyOptions;
        private readonly ICurrentTenant _currentTenant;

        public MultiTenancyStepMiddleware(
            ICurrentTenant currentTenant,
            IOptions<AbpMultiTenancyOptions> options)
        {
            _currentTenant = currentTenant;
            _multiTenancyOptions = options.Value;
        }

        public async Task<ExecutionResult> HandleAsync(IStepExecutionContext context, IStepBody body, WorkflowStepDelegate next)
        {
            // 触发多租户中间件条件
            // 1、需要启用多租户
            // 2、步骤需要实现接口 IStepMultiTenant
            // 3.1、传递的工作流实现接口 IMultiTenant
            // 3.2、传递的工作流数据包含 TenantId 字段

            if (!_multiTenancyOptions.IsEnabled)
            {
                return await next();
            }

            if (typeof(IStepMultiTenant).IsAssignableFrom(body.GetType()))
            {
                if (context.Workflow.Data != null)
                {
                    if (context.Workflow.Data is IMultiTenant tenant)
                    {
                        using (_currentTenant.Change(tenant.TenantId))
                        {
                            return await next();
                        }
                    }

                    var dataObj = JObject.FromObject(context.Workflow.Data);
                    var tenantToken = dataObj.GetOrDefault(nameof(IMultiTenant.TenantId));
                    if (tenantToken?.HasValues == true)
                    {
                        using (_currentTenant.Change(tenantToken.Value<Guid?>()))
                        {
                            return await next();
                        }
                    }
                }
            }

            return await next();
        }
    }
}
