using LINGYUN.Platform.Localization;
using LINGYUN.Platform.Versions;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Platform.Settings
{
    public class PlatformSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(CreateAppVersionSettings());
        }

        protected SettingDefinition[] CreateAppVersionSettings()
        {
            return new SettingDefinition[]
            {
                new SettingDefinition(PlatformSettingNames.AppVersion.VersionFileLimitLength, AppVersionConsts.DefaultVersionFileLimitLength.ToString(),
                    L("DisplayName:VersionFileLimitLength"), L("Description:VersionFileLimitLength"), isVisibleToClients: true),
                new SettingDefinition(PlatformSettingNames.AppVersion.AllowVersionFileExtensions, AppVersionConsts.DefaultAllowVersionFileExtensions,
                    L("DisplayName:AllowVersionFileExtensions"), L("Description:AllowVersionFileExtensions"), isVisibleToClients: true),
            };
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<PlatformResource>(name);
        }
    }
}
