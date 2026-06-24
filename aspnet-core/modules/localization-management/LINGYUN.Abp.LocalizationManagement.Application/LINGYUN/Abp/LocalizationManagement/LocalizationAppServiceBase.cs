using LINGYUN.Abp.LocalizationManagement.Localization;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.EventBus.Local;

namespace LINGYUN.Abp.LocalizationManagement;

public abstract class LocalizationAppServiceBase : ApplicationService
{
    protected ILocalEventBus LocalEventBus => LazyServiceProvider.LazyGetService<ILocalEventBus>();
    protected LocalizationAppServiceBase()
    {
        LocalizationResource = typeof(LocalizationManagementResource);
        ObjectMapperContext = typeof(AbpLocalizationManagementApplicationModule);
    }

    protected async virtual Task PublishDynamicLocalizationRefreshEvent<TEvent>(TEvent @event)
    {
        await LocalEventBus?.PublishAsync(@event.GetType(), @event);
    }
}
