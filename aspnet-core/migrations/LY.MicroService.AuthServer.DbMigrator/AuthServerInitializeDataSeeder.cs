using LY.MicroService.AuthServer.DbMigrator.DataSeeds;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LY.MicroService.AuthServer.DbMigrator;

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

