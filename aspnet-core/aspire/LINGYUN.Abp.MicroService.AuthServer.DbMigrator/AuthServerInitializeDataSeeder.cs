using LINGYUN.Abp.MicroService.AuthServer.DataSeeds;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MicroService.AuthServer;

public class AuthServerInitializeDataSeeder : ITransientDependency
{
    protected IdentityUserNotificationDataSeeder UserNotificationDataSeeder { get; }
    public AuthServerInitializeDataSeeder(IdentityUserNotificationDataSeeder userNotificationDataSeeder)
    {
        UserNotificationDataSeeder = userNotificationDataSeeder;
    }

    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await UserNotificationDataSeeder.SeedAsync(context);
    }
}
