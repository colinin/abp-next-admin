using LINGYUN.Platform.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Settings;

namespace LINGYUN.Platform
{
    public abstract class PlatformControllerBase : AbpController
    {
        private ISettingProvider _settingProvider;
        protected ISettingProvider SettingProvider => LazyGetRequiredService(ref _settingProvider);

        protected PlatformControllerBase()
        {
            LocalizationResource = typeof(PlatformResource);
        }
    }
}
