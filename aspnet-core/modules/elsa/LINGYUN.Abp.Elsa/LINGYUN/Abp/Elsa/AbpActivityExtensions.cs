using Elsa.Builders;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa;

public static class AbpActivityExtensions
{
    public static ISetupActivity<AbpActivity> WithTenantId(
        this ISetupActivity<AbpActivity> activity,
        Func<ActivityExecutionContext, ValueTask<Guid?>> value) => activity.Set(x => x.TenantId, value);

    public static ISetupActivity<AbpActivity> WithTenantId(
        this ISetupActivity<AbpActivity> activity,
        Func<ActivityExecutionContext, Guid?> value) => activity.Set(x => x.TenantId, value);

    public static ISetupActivity<AbpActivity> WithTenantId(
        this ISetupActivity<AbpActivity> activity,
        Func<Guid?> value) => activity.Set(x => x.TenantId, value);

    public static ISetupActivity<AbpActivity> WithTenantId(
        this ISetupActivity<AbpActivity> activity,
        Guid? value) => activity.Set(x => x.TenantId, value);
}
