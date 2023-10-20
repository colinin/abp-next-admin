using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public static class ISettingProviderExtensions
    {
        public static async Task<string> GetOrDefaultAsync(
            [NotNull] this ISettingProvider settingProvider,
            [NotNull] string name,
            [NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(settingProvider, nameof(settingProvider));
            Check.NotNull(serviceProvider, nameof(serviceProvider));
            Check.NotNullOrWhiteSpace(name, nameof(name));
            var value = await settingProvider.GetOrNullAsync(name);
            if (value.IsNullOrWhiteSpace())
            {
                var settingDefintionManager = serviceProvider.GetRequiredService<ISettingDefinitionManager>();
                var setting = await settingDefintionManager.GetAsync(name);
                return setting.DefaultValue;
            }
            return value;
        }
    }
}
