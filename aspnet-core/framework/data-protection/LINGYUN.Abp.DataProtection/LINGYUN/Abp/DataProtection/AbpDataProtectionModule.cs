using LINGYUN.Abp.DataProtection.Keywords;
using LINGYUN.Abp.DataProtection.Localization;
using LINGYUN.Abp.DataProtection.Operations;
using LINGYUN.Abp.DataProtection.Subjects;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtection;

[DependsOn(
    typeof(AbpDataProtectionAbstractionsModule),
    typeof(AbpDddDomainModule))]
public class AbpDataProtectionModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(DataProtectedInterceptorRegistrar.RegisterIfNeeded);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDataProtectionOptions>(options =>
        {
            options.KeywordContributors.Add(DataAccessCurrentUserContributor.Name, new DataAccessCurrentUserContributor());

            options.OperateContributors.Add(DataAccessFilterOperate.Equal, new DataAccessEqualContributor());
            options.OperateContributors.Add(DataAccessFilterOperate.NotEqual, new DataAccessNotEqualContributor());
            options.OperateContributors.Add(DataAccessFilterOperate.Less, new DataAccessLessContributor());
            options.OperateContributors.Add(DataAccessFilterOperate.LessOrEqual, new DataAccessLessOrEqualContributor());
            options.OperateContributors.Add(DataAccessFilterOperate.Greater, new DataAccessGreaterContributor());
            options.OperateContributors.Add(DataAccessFilterOperate.GreaterOrEqual, new DataAccessGreaterOrEqualContributor());
            options.OperateContributors.Add(DataAccessFilterOperate.StartsWith, new DataAccessStartsWithContributor());
            options.OperateContributors.Add(DataAccessFilterOperate.EndsWith, new DataAccessEndsWithContributor());
            options.OperateContributors.Add(DataAccessFilterOperate.Contains, new DataAccessContainsContributor());
            options.OperateContributors.Add(DataAccessFilterOperate.NotContains, new DataAccessNotContainsContributor());

            options.SubjectContributors.Add(new DataAccessUserIdContributor());
            options.SubjectContributors.Add(new DataAccessRoleNameContributor());
            options.SubjectContributors.Add(new DataAccessOrganizationUnitContributor());
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Add<DataProtectionResource>();
        });
    }
}
