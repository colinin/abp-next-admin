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
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpDddDomainModule),
    typeof(AbpLocalizationManagementDomainSharedModule))]
public class AbpLocalizationManagementDomainModule : AbpModule
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpLocalizationManagementDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<LocalizationManagementDomainMapperProfile>(validate: true);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.GlobalContributors.Add<LocalizationResourceContributor>();
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<Text, TextEto>();
            options.EtoMappings.Add<Language, LanguageEto>();
            options.EtoMappings.Add<Resource, ResourceEto>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await SaveLocalizationAsync(context);
    }

    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        cancellationTokenSource.CancelAsync();
        return Task.CompletedTask;
    }

    private async Task SaveLocalizationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpLocalizationManagementOptions>>();
        if (options.Value.SaveStaticLocalizationsToDatabase)
        {
            var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
            await Task.Run(async () =>
            {
                using (var scope = rootServiceProvider.CreateScope())
                {
                    var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
                    var cancellationTokenProvider = scope.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
                    var cancellationToken = applicationLifetime?.ApplicationStopping ?? cancellationTokenSource.Token;
                    try
                    {
                        using (cancellationTokenProvider.Use(cancellationToken))
                        {
                            if (cancellationTokenProvider.Token.IsCancellationRequested)
                            {
                                return;
                            }

                            await Policy.Handle<Exception>()
                                .WaitAndRetryAsync(8, 
                                    retryAttempt => TimeSpan.FromSeconds(
                                        RandomHelper.GetRandom((int)Math.Pow(2.0, retryAttempt) * 8, (int)Math.Pow(2.0, retryAttempt) * 12)))
                                .ExecuteAsync(async _ =>
                                {
                                    try
                                    {
                                        await scope.ServiceProvider
                                            .GetRequiredService<IStaticLocalizationSaver>()
                                            .SaveAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                        scope.ServiceProvider
                                            .GetService<ILogger<AbpLocalizationModule>>()
                                            ?.LogException(ex);

                                        throw;
                                    }
                                },
                                cancellationTokenProvider.Token);
                        }
                    }
                    catch
                    {
                    }
                }
            });
        }
    }
}
