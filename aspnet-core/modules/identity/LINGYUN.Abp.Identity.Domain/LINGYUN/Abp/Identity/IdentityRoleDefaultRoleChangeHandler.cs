using LINGYUN.Abp.Identity.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;
using Volo.Abp.Roles;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Identity;

public class IdentityRoleDefaultRoleChangeHandler :
    ILocalEventHandler<EntityChangedEventData<IdentityRole>>,
    ITransientDependency
{
    protected ISettingProvider SettingProvider { get; }
    public IdentityRoleDefaultRoleChangeHandler(ISettingProvider settingProvider)
    {
        SettingProvider = settingProvider;
    }
    public async virtual Task HandleEventAsync(EntityChangedEventData<IdentityRole> eventData)
    {
        if (eventData.Entity.Name.Equals(AbpRoleConsts.AdminRoleName, StringComparison.CurrentCultureIgnoreCase) &&
            await SettingProvider.IsTrueAsync(IdentitySettingNames.TwoFactor.ProhibitDefaultAdminRole))
        {
            throw new BusinessException(
                IdentityErrorCodes.ProhibitSetDefaultAdminRole,
                "Prohibit setting the administrator role as the default role!");
        }
    }
}
