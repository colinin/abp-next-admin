using JetBrains.Annotations;
using System.Linq;
using System.Reflection;
using Volo.Abp;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore
{
    public static class WorkflowRegisterHelper
    {
        public readonly static MethodInfo RegisterGenericWorkflowMethod =
            typeof(IWorkflowRegistry)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .First(m => m.Name == nameof(IWorkflowRegistry.RegisterWorkflow) && m.IsGenericMethodDefinition);

        public static void RegisterWorkflow(
            [NotNull] IWorkflowRegistry registry,
            [NotNull] object workflow)
        {
            Check.NotNull(registry, nameof(registry));
            Check.NotNull(workflow, nameof(workflow));

            var workflowDataType = workflow.GetType()
                .GetInterfaces()
                .First(x => x.IsGenericType)
                .GetGenericArguments()[0];

            RegisterGenericWorkflowMethod
                .MakeGenericMethod(workflowDataType)
                .Invoke(registry, new object[] { workflow });
        }
    }
}
