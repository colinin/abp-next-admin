using LINGYUN.Platform.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Platform.Settings
{
    public class PlatformSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(CreateDefaultSettings());
        }

        protected SettingDefinition[] CreateDefaultSettings()
        {
            return new SettingDefinition[0];
        }

        protected LocalizableString L(string name)
        {
            return LocalizableString.Create<PlatformResource>(name);
        }
    }
}
