using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Options;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Identity
{
    public class AbpIdentityOverrideOptionsFactory : AbpOptionsFactory<IdentityOptions>
    {
        protected ISettingStore SettingStore { get; }
        public AbpIdentityOverrideOptionsFactory(
            ISettingStore settingStore,
            IEnumerable<IConfigureOptions<IdentityOptions>> setups,
            IEnumerable<IPostConfigureOptions<IdentityOptions>> postConfigures)
            : base(setups, postConfigures)
        {
            SettingStore = settingStore;
        }

        public override IdentityOptions Create(string name)
        {
            var options = base.Create(name);

            // 重写为只获取公共配置
            OverrideOptions(options);

            return options;
        }

        protected virtual void OverrideOptions(IdentityOptions options)
        {
            AsyncHelper.RunSync(() => OverrideOptionsAsync(options));
        }

        protected virtual async Task OverrideOptionsAsync(IdentityOptions options)
        {
            options.Password.RequiredLength = await GetOrDefaultAsync(IdentitySettingNames.Password.RequiredLength, options.Password.RequiredLength);
            options.Password.RequiredUniqueChars = await GetOrDefaultAsync(IdentitySettingNames.Password.RequiredUniqueChars, options.Password.RequiredUniqueChars);
            options.Password.RequireNonAlphanumeric = await GetOrDefaultAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, options.Password.RequireNonAlphanumeric);
            options.Password.RequireLowercase = await GetOrDefaultAsync(IdentitySettingNames.Password.RequireLowercase, options.Password.RequireLowercase);
            options.Password.RequireUppercase = await GetOrDefaultAsync(IdentitySettingNames.Password.RequireUppercase, options.Password.RequireUppercase);
            options.Password.RequireDigit = await GetOrDefaultAsync(IdentitySettingNames.Password.RequireDigit, options.Password.RequireDigit);

            options.Lockout.AllowedForNewUsers = await GetOrDefaultAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, options.Lockout.AllowedForNewUsers);
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(await GetOrDefaultAsync(IdentitySettingNames.Lockout.LockoutDuration, options.Lockout.DefaultLockoutTimeSpan.TotalSeconds.To<int>()));
            options.Lockout.MaxFailedAccessAttempts = await GetOrDefaultAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, options.Lockout.MaxFailedAccessAttempts);

            options.SignIn.RequireConfirmedEmail = await GetOrDefaultAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, options.SignIn.RequireConfirmedEmail);
            options.SignIn.RequireConfirmedPhoneNumber = await GetOrDefaultAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, options.SignIn.RequireConfirmedPhoneNumber);
        }

        protected virtual async Task<T> GetOrDefaultAsync<T>(string name, T defaultValue = default(T)) where T : struct
        {
            var setting = await SettingStore.GetOrNullAsync(name, GlobalSettingValueProvider.ProviderName, null);
            if (setting.IsNullOrWhiteSpace())
            {
                return defaultValue;
            }
            return setting.To<T>();
        }

        protected virtual async Task<string> GetOrDefaultAsync(string name, string defaultValue = "")
        {
            var setting = await SettingStore.GetOrNullAsync(name, GlobalSettingValueProvider.ProviderName, null);
            if (setting.IsNullOrWhiteSpace())
            {
                return defaultValue;
            }
            return setting;
        }
    }
}
