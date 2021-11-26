using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ActionContextExtensions
    {
        public static bool CanWarpRsult(this ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor is ControllerActionDescriptor descriptor)
            {
                if (descriptor.MethodInfo.IsDefined(typeof(IgnoreWrapResultAttribute), true))
                {
                    return false;
                }

                if (descriptor.ControllerTypeInfo.IsDefined(typeof(IgnoreWrapResultAttribute), true))
                {
                    return false;
                }

                return true;
            }
            return false;
        }
    }
}
