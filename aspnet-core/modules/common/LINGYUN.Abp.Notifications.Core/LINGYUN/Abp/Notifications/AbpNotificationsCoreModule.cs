using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications;

[DependsOn(
    typeof(AbpTextTemplatingCoreModule))]
public class AbpNotificationsCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var preActions = context.Services.GetPreConfigureActions<AbpNotificationsOptions>();
        Configure<AbpNotificationsOptions>(options =>
        {
            preActions.Configure(options);
        });
    }

    private void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistred(context =>
        {
            if (typeof(INotificationDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        Configure<AbpNotificationsOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}
