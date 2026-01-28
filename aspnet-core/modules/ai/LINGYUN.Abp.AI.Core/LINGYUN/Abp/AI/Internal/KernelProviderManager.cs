using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Internal;
public class KernelProviderManager : IKernelProviderManager, ISingletonDependency
{
    public List<IKernelProvider> Providers => _lazyProviders.Value;

    protected AbpAICoreOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }
    private readonly Lazy<List<IKernelProvider>> _lazyProviders;

    public KernelProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpAICoreOptions> options)
    {

        Options = options.Value;
        ServiceProvider = serviceProvider;

        _lazyProviders = new Lazy<List<IKernelProvider>>(GetProviders, true);
    }

    protected virtual List<IKernelProvider> GetProviders()
    {
        var providers = Options
            .ChatClientProviders
            .Select(type => (ServiceProvider.GetRequiredService(type) as IKernelProvider)!)
            .ToList();

        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        if (multipleProviders != null)
        {
            throw new AbpException($"Duplicate Kernel provider name detected: {multipleProviders.Key}. Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}");
        }

        return providers;
    }
}
