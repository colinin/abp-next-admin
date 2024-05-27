using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.TextTemplating;
public class TextTemplateDefinitionInitializer : ITransientDependency
{
    protected IRootServiceProvider RootServiceProvider { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpTextTemplatingCachingOptions TextTemplatingCachingOptions { get; }

    public TextTemplateDefinitionInitializer(
        IRootServiceProvider rootServiceProvider, 
        ICancellationTokenProvider cancellationTokenProvider,
        IOptions<AbpTextTemplatingCachingOptions> textTemplatingCachingOptions)
    {
        RootServiceProvider = rootServiceProvider;
        CancellationTokenProvider = cancellationTokenProvider;
        TextTemplatingCachingOptions = textTemplatingCachingOptions.Value;
    }

    public async virtual Task InitializeDynamicTemplates(CancellationToken cancellationToken)
    {
        if (!TextTemplatingCachingOptions.SaveStaticTemplateDefinitionToDatabase && !TextTemplatingCachingOptions.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return;
        }

        using var scope = RootServiceProvider.CreateScope();
        var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
        var token = applicationLifetime?.ApplicationStopping ?? cancellationToken;
        using (CancellationTokenProvider.Use(cancellationToken))
        {
            if (CancellationTokenProvider.Token.IsCancellationRequested)
            {
                return;
            }

            await SaveStaticTemplateDefinitionsToDatabaseAsync(scope);

            if (CancellationTokenProvider.Token.IsCancellationRequested)
            {
                return;
            }

            await PreCacheDynamicTemplateDefinitionsAsync(scope);
        }
    }

    private async Task SaveStaticTemplateDefinitionsToDatabaseAsync(IServiceScope serviceScope)
    {
        if (!TextTemplatingCachingOptions.SaveStaticTemplateDefinitionToDatabase)
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
                    var saver = serviceScope.ServiceProvider.GetRequiredService<IStaticTemplateSaver>();

                    await saver.SaveDefinitionTemplateAsync();

                    await saver.SaveTemplateContentAsync();
                }
                catch (Exception ex)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    serviceScope.ServiceProvider
                        .GetService<ILogger<TextTemplateDefinitionInitializer>>()?
                        .LogException(ex);

                    throw; // Polly will catch it
                }
            }, CancellationTokenProvider.Token);
       
    }

    private async Task PreCacheDynamicTemplateDefinitionsAsync(IServiceScope serviceScope)
    {
        if (!TextTemplatingCachingOptions.IsDynamicTemplateDefinitionStoreEnabled)
        {
            return;
        }

        try
        {
            // ReSharper disable once AccessToDisposedClosure
            var store = serviceScope.ServiceProvider.GetRequiredService<ITemplateDefinitionStore>();

            await store.GetAllAsync();
        }
        catch (Exception ex)
        {
            // ReSharper disable once AccessToDisposedClosure
            serviceScope.ServiceProvider
                .GetService<ILogger<TextTemplateDefinitionInitializer>>()?
                .LogException(ex);

            throw; // Polly will catch it
        }
    }
}
