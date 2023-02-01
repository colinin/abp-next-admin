using Elsa.Builders;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa;

public static class AbpActivityExtensions
{
    public static ISetupActivity<TActivity> WithTenantId<TActivity>(
        this ISetupActivity<TActivity> activity,
        Func<ActivityExecutionContext, ValueTask<Guid?>> value) where TActivity : AbpActivity => activity.Set(x => x.TenantId, value);

    public static ISetupActivity<TActivity> WithTenantId<TActivity>(
        this ISetupActivity<TActivity> activity,
        Func<ActivityExecutionContext, Guid?> value) where TActivity : AbpActivity => activity.Set(x => x.TenantId, value);

    public static ISetupActivity<TActivity> WithTenantId<TActivity>(
        this ISetupActivity<TActivity> activity,
        Func<Guid?> value) where TActivity : AbpActivity => activity.Set(x => x.TenantId, value);

    public static ISetupActivity<TActivity> WithTenantId<TActivity>(
        this ISetupActivity<TActivity> activity,
        Guid? value) where TActivity : AbpActivity => activity.Set(x => x.TenantId, value);
}
