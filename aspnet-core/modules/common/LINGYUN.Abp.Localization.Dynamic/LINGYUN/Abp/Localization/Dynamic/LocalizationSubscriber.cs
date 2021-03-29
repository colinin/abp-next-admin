using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Localization.Dynamic
{
    [ExposeServices(
        typeof(ILocalizationSubscriber),
        typeof(ILocalizationDispatcher),
        typeof(LocalizationSubscriber))]
    internal class LocalizationSubscriber : ILocalizationSubscriber, ILocalizationDispatcher, ISingletonDependency
    {
        private Func<LocalizedStringCacheResetEventData, Task> _handler;

        public LocalizationSubscriber()
        {
            _handler = (d) =>
            {
                return Task.CompletedTask;
            };
        }

        public void Subscribe(Func<LocalizedStringCacheResetEventData, Task> func)
        {
            _handler += func;
        }

        public virtual async Task DispatchAsync(LocalizedStringCacheResetEventData data)
        {
            await _handler(data);
        }
    }
}
