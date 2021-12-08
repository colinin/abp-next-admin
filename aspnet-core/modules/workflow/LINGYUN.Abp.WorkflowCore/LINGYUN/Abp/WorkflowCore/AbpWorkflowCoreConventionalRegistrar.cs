using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WorkflowCore
{
    public class AbpWorkflowCoreConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !IsWorkflowComponent(type) || base.IsConventionalRegistrationDisabled(type);
        }

        private static bool IsWorkflowComponent(Type type)
        {
            return type.IsWorkflow() || type.IsStepBody();
        }

        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }
}
