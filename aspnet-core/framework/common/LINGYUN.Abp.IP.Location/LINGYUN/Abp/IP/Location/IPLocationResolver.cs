using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IP.Location;
public class IPLocationResolver : IIPLocationResolver, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AbpIPLocationResolveOptions _options;

    public IPLocationResolver(IOptions<AbpIPLocationResolveOptions> options, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    public virtual async Task<IPLocationResolveResult> ResolveAsync(string ipAddress)
    {
        var result = new IPLocationResolveResult();

        using (var serviceScope = _serviceProvider.CreateScope())
        {
            var context = new IPLocationResolveContext(ipAddress, serviceScope.ServiceProvider);

            foreach (var ipLocationResolver in _options.IPLocationResolvers)
            {
                await ipLocationResolver.ResolveAsync(context);

                result.AppliedResolvers.Add(ipLocationResolver.Name);

                if (context.HasResolvedIPLocation())
                {
                    result.Location = context.Location;
                    break;
                }
            }
        }

        return result;
    }
}
