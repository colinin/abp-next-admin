using LINGYUN.Abp.MicroService.MessageService.DataSeeds;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MicroService.MessageService;

public class MessageServiceDataSeeder : ITransientDependency
{
    protected NotificationDataSeeder NotificationDataSeeder { get; }
    protected ICurrentTenant CurrentTenant { get; }
    public MessageServiceDataSeeder(
        ICurrentTenant currentTenant,
        NotificationDataSeeder notificationDataSeeder)
    {
        CurrentTenant = currentTenant;
        NotificationDataSeeder = notificationDataSeeder;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        using (CurrentTenant.Change(context.TenantId))
        {
            await SeedNotificationAsync(context);
        }
    }

    private async Task SeedNotificationAsync(DataSeedContext context)
    {
        await NotificationDataSeeder.SeedAsync(context);
    }
}
