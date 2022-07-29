using System;

namespace Elsa.Models;

public static class WorkflowInstanceExtensions
{
    public static Guid? GetTenantId(this WorkflowInstance instance)
    {
        var tenantId = instance.TenantId;
        if (!tenantId.IsNullOrWhiteSpace() &&
            Guid.TryParse(tenantId, out var id))
        {
            return id;
        }
        return null;
    }
}
