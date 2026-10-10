using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
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

        if (!IPAddress.TryParse(ipAddress, out var ip))
        {
            // 非法地址不解析
            return result;
        }

        // 本地回环与未指定地址(离线库无记录, 例如 127.0.0.1、::1、::)
        if (IPAddress.IsLoopback(ip) || ip.Equals(IPAddress.Any) || ip.Equals(IPAddress.IPv6Any))
        {
            return result;
        }

        using (var serviceScope = _serviceProvider.CreateScope())
        {
            var context = new IPLocationResolveContext(ip, serviceScope.ServiceProvider);

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
