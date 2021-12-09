using System.Linq;
using WorkflowCore.Interface;

namespace System
{
    public static class WorkflowTypeExtensions
    {
        public static bool IsWorkflow(this Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IWorkflow<>));
        }

        public static bool IsStepBody(this Type type)
        {
            return typeof(IStepBody).IsAssignableFrom(type);
        }
    }
}
