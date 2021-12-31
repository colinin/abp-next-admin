using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Tencent.SettingManagement;

public class TenantCloudSettingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var wechatGroup = context.AddGroup(
            TenantCloudSettingPermissionNames.GroupName,
            L("Permission:TencentCloud"));

        wechatGroup.AddPermission(
            TenantCloudSettingPermissionNames.Settings, L("Permission:TencentCloud.Settings"));
    }

    protected LocalizableString L(string name)
    {
        return LocalizableString.Create<TencentCloudResource>(name);
    }
}
