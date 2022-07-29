using Elsa.Models;
using System;

namespace Elsa.Services.Models;

public static class ActivityExecutionContextExtensions
{
    public static Guid? GetTenantId(this ActivityExecutionContext context)
    {
        return context.WorkflowInstance.GetTenantId();
    }
}
