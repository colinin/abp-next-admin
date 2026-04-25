using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tools;
public class AIToolProviderManager : IAIToolProviderManager, ISingletonDependency
{
    public List<IAIToolProvider> Providers => _lazyProviders.Value;

    protected AbpAIToolsOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }

    private readonly Lazy<List<IAIToolProvider>> _lazyProviders;

    public AIToolProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpAIToolsOptions> options)
    {

        Options = options.Value;
        ServiceProvider = serviceProvider;

        _lazyProviders = new Lazy<List<IAIToolProvider>>(GetProviders, true);
    }

    protected virtual List<IAIToolProvider> GetProviders()
    {
        var providers = Options
            .AIToolProviders
            .Select(type => (ServiceProvider.GetRequiredService(type) as IAIToolProvider)!)
            .ToList();

        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        if (multipleProviders != null)
        {
            throw new AbpException($"Duplicate AITool provider name detected: {multipleProviders.Key}. Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}");
        }

        return providers;
    }
}
