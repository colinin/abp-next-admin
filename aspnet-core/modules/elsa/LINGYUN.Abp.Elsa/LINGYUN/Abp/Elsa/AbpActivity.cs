using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Elsa;

public abstract class AbpActivity : Activity, IMultiTenantActivitySupport
{
    [ActivityInput(Hint = "The tenant id.")]
    public Guid? TenantId { get; set; }

    protected override ValueTask<bool> OnCanExecuteAsync(ActivityExecutionContext context)
    {
        var currentTenant = context.GetService<ICurrentTenant>();
        using (currentTenant.Change(TenantId ?? context.GetTenantId() ?? currentTenant.Id))
        {
            return OnActivityCanExecuteAsync(context);
        }
    }

    protected override ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        var currentTenant = context.GetService<ICurrentTenant>();
        using (currentTenant.Change(TenantId ?? context.GetTenantId() ?? currentTenant.Id))
        {
            return OnActivityExecuteAsync(context);
        }
    }

    protected virtual ValueTask<bool> OnActivityCanExecuteAsync(ActivityExecutionContext context)
    {
        return new ValueTask<bool>(OnCanExecute(context));
    }

    protected virtual ValueTask<IActivityExecutionResult> OnActivityExecuteAsync(ActivityExecutionContext context)
    {
        return new ValueTask<IActivityExecutionResult>(OnExecute(context));
    }
}
