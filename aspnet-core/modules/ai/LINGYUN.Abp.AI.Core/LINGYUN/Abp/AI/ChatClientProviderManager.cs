using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI;
public class ChatClientProviderManager : IChatClientProviderManager, ISingletonDependency
{
    public List<IChatClientProvider> Providers => _lazyProviders.Value;

    protected AbpAICoreOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }
    private readonly Lazy<List<IChatClientProvider>> _lazyProviders;

    public ChatClientProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpAICoreOptions> options)
    {

        Options = options.Value;
        ServiceProvider = serviceProvider;

        _lazyProviders = new Lazy<List<IChatClientProvider>>(GetProviders, true);
    }

    protected virtual List<IChatClientProvider> GetProviders()
    {
        var providers = Options
            .ChatClientProviders
            .Select(type => (ServiceProvider.GetRequiredService(type) as IChatClientProvider)!)
            .ToList();

        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        if (multipleProviders != null)
        {
            throw new AbpException($"Duplicate ChatClient provider name detected: {multipleProviders.Key}. Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}");
        }

        return providers;
    }
}
