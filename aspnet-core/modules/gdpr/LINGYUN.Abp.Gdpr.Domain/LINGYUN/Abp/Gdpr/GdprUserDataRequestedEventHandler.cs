using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Gdpr;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Gdpr;

/// <summary>
/// 用户个人数据收集事件监听器<br />
/// See: https://abp.io/docs/latest/modules/gdpr#gdpruserdatarequestedeto
/// </summary>
/// <param name="options"></param>
/// <param name="serviceScopeFactory"></param>
public class GdprUserDataRequestedEventHandler(
    IOptions<AbpGdprOptions> options,
    IServiceScopeFactory serviceScopeFactory
) : IDistributedEventHandler<GdprUserDataRequestedEto>,
    IDistributedEventHandler<GdprUserDataDeletionRequestedEto>,
    IDistributedEventHandler<GdprUserAccountDeletionRequestedEto>,
    ITransientDependency
{
    public async virtual Task HandleEventAsync(GdprUserDataRequestedEto eventData)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var context = new GdprPrepareUserDataContext(eventData.RequestId, scope.ServiceProvider)
        {
            UserId = eventData.UserId,
        };

        foreach (var gdprUserDataProvider in options.Value.GdprUserDataProviders)
        {
            await gdprUserDataProvider.PorepareAsync(context);
        }
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(GdprUserDataDeletionRequestedEto eventData)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var context = new GdprDeleteUserDataContext(scope.ServiceProvider)
        {
            UserId = eventData.UserId,
        };

        foreach (var gdprUserDataProvider in options.Value.GdprUserDataProviders)
        {
            await gdprUserDataProvider.DeleteAsync(context);
        }
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(GdprUserAccountDeletionRequestedEto eventData)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var context = new GdprDeleteUserAccountContext(scope.ServiceProvider)
        {
            UserId = eventData.UserId,
        };

        foreach (var gdprUserAccountProvider in options.Value.GdprUserAccountProviders)
        {
            await gdprUserAccountProvider.DeleteAsync(context);
        }
    }
}
