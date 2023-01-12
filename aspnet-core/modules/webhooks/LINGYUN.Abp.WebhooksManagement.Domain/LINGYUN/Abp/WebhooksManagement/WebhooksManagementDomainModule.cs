using LINGYUN.Abp.Webhooks;
using LINGYUN.Abp.WebhooksManagement.ObjectExtending;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.WebhooksManagement;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpWebhooksModule),
    typeof(WebhooksManagementDomainSharedModule))]
public class WebhooksManagementDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly static OneTimeRunner OneTimeRunner = new();
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<WebhooksManagementDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<WebhooksManagementDomainMapperProfile>(validate: true);
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<WebhookEventRecord, WebhookEventEto>();
            options.EtoMappings.Add<WebhookSendRecord, WebhookSendAttemptEto>();
            options.EtoMappings.Add<WebhookSubscription, WebhookSubscriptionEto>();

            options.AutoEventSelectors.Add<WebhookEventRecord>();
            options.AutoEventSelectors.Add<WebhookSendRecord>();
            options.AutoEventSelectors.Add<WebhookSubscription>();
        });

        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<WebhookManagementOptions>(options =>
            {
                options.SaveStaticWebhooksToDatabase = false;
                options.IsDynamicWebhookStoreEnabled = false;
            });
        }
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            // 扩展实体配置
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                WebhooksManagementModuleExtensionConsts.ModuleName,
                WebhooksManagementModuleExtensionConsts.EntityNames.WebhookEvent,
                typeof(WebhookEventRecord)
            );
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                WebhooksManagementModuleExtensionConsts.ModuleName,
                WebhooksManagementModuleExtensionConsts.EntityNames.WebhookSubscription,
                typeof(WebhookSubscription)
            );
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                WebhooksManagementModuleExtensionConsts.ModuleName,
                WebhooksManagementModuleExtensionConsts.EntityNames.WebhookSendAttempt,
                typeof(WebhookSendRecord)
            );
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                WebhooksManagementModuleExtensionConsts.ModuleName,
                WebhooksManagementModuleExtensionConsts.EntityNames.WebhookGroupDefinition,
                typeof(WebhookGroupDefinitionRecord)
            );
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                WebhooksManagementModuleExtensionConsts.ModuleName,
                WebhooksManagementModuleExtensionConsts.EntityNames.WebhookDefinition,
                typeof(WebhookDefinitionRecord)
            );
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        InitializeDynamicWebhooks(context);
        return Task.CompletedTask;
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    private void InitializeDynamicWebhooks(ApplicationInitializationContext context)
    {
        var options = context
            .ServiceProvider
            .GetRequiredService<IOptions<WebhookManagementOptions>>()
            .Value;

        if (!options.SaveStaticWebhooksToDatabase && !options.IsDynamicWebhookStoreEnabled)
        {
            return;
        }

        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        Task.Run(async () =>
        {
            using var scope = rootServiceProvider.CreateScope();
            var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationTokenProvider = scope.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
            var cancellationToken = applicationLifetime?.ApplicationStopping ?? _cancellationTokenSource.Token;

            try
            {
                using (cancellationTokenProvider.Use(cancellationToken))
                {
                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await SaveStaticWebhooksToDatabaseAsync(options, scope, cancellationTokenProvider);

                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await PreCacheDynamicWebhooksAsync(options, scope);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch { }
        });
    }

    private async static Task SaveStaticWebhooksToDatabaseAsync(
        WebhookManagementOptions options,
        IServiceScope scope,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        if (!options.SaveStaticWebhooksToDatabase)
        {
            return;
        }

        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(8, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * 10))
            .ExecuteAsync(async _ =>
            {
                try
                {
                    // ReSharper disable once AccessToDisposedClosure
                    await scope
                        .ServiceProvider
                        .GetRequiredService<IStaticWebhookSaver>()
                        .SaveAsync();
                }
                catch (Exception ex)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    scope.ServiceProvider
                        .GetService<ILogger<WebhooksManagementDomainModule>>()?
                        .LogException(ex);

                    throw; // Polly will catch it
                }
            }, cancellationTokenProvider.Token);
    }

    private async static Task PreCacheDynamicWebhooksAsync(WebhookManagementOptions options, IServiceScope scope)
    {
        if (!options.IsDynamicWebhookStoreEnabled)
        {
            return;
        }

        try
        {
            // Pre-cache permissions, so first request doesn't wait
            await scope
                .ServiceProvider
                .GetRequiredService<IDynamicWebhookDefinitionStore>()
                .GetGroupsAsync();
        }
        catch (Exception ex)
        {
            // ReSharper disable once AccessToDisposedClosure
            scope
                .ServiceProvider
                .GetService<ILogger<WebhooksManagementDomainModule>>()?
                .LogException(ex);

            throw; // It will be cached in InitializeDynamicWebhooks
        }
    }
}
