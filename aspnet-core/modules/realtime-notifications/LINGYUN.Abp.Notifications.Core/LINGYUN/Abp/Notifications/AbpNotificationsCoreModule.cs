using LINGYUN.Abp.Notifications.Localization;
using LINGYUN.Abp.RealTime;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.EventBus.Abstractions;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Notifications;

[DependsOn(
    typeof(AbpTextTemplatingCoreModule),
    typeof(AbpRealTimeModule),
    typeof(AbpLocalizationModule),
    typeof(AbpEventBusAbstractionsModule),
    typeof(AbpMultiTenancyAbstractionsModule))]
public class AbpNotificationsCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Add<NotificationsResource>();
        });

        var preActions = context.Services.GetPreConfigureActions<AbpNotificationsOptions>();
        Configure<AbpNotificationsOptions>(options =>
        {
            preActions.Configure(options);
        });
    }

    private void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistered(context =>
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
