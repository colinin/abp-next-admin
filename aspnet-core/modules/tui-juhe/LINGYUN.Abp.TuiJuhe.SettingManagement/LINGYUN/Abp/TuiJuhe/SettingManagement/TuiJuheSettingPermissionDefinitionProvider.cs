using LINGYUN.Abp.TuiJuhe.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.TuiJuhe.SettingManagement
{
    public class WxPusherSettingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var tuiJuheGroup = context.AddGroup(
                TuiJuheSettingPermissionNames.GroupName,
                L("Permission:TuiJuhe"));

            tuiJuheGroup.AddPermission(
                TuiJuheSettingPermissionNames.ManageSetting, L("Permission:ManageSetting"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<TuiJuheResource>(name);
        }
    }
}
