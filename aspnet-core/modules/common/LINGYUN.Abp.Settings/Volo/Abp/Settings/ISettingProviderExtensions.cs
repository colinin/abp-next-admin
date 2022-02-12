using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Volo.Abp.Settings
{
    public static class ISettingProviderExtensions
    {
        public async static Task<string> GetOrDefaultAsync(
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
                var settingDefinitionManager = serviceProvider.GetRequiredService<ISettingDefinitionManager>();
                var setting = settingDefinitionManager.Get(name);
                return setting.DefaultValue;
            }
            return value;
        }
    }
}
