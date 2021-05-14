using LINGYUN.Abp.AspNetCore.HttpOverrides.Forwarded;
using Microsoft.AspNetCore.HttpOverrides;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Microsoft.AspNetCore.Builder
{
    public static class ForwardedHeadersOptionsExtensions
    {
        public static void Configure(
            this ForwardedHeadersOptions options,
            AbpForwardedHeadersOptions abpOptions)
        {
            options.ForwardedForHeaderName = abpOptions.ForwardedForHeaderName;
            options.ForwardedHeaders = abpOptions.ForwardedHeaders;
            options.ForwardedHostHeaderName = abpOptions.ForwardedHostHeaderName;
            options.ForwardedProtoHeaderName = abpOptions.ForwardedProtoHeaderName;
            options.ForwardLimit = abpOptions.ForwardLimit;
            options.OriginalForHeaderName = abpOptions.OriginalForHeaderName;
            options.OriginalHostHeaderName = abpOptions.OriginalHostHeaderName;
            options.OriginalProtoHeaderName = abpOptions.OriginalProtoHeaderName;
            options.RequireHeaderSymmetry = abpOptions.RequireHeaderSymmetry;

            if (abpOptions.AllowedHosts.Any())
            {
                options.AllowedHosts = abpOptions.AllowedHosts;
            }

            AddProxies(options.KnownProxies, abpOptions.KnownProxies);

            if (abpOptions.KnownNetworks.Any())
            {
                options.KnownNetworks.Clear();
                foreach (var proxy in abpOptions.KnownNetworks)
                {
                    // 192.168.1.0/24
                    var spiltProxies = proxy.Split("/");
                    if (spiltProxies.Length != 2)
                    {
                        continue;
                    }
                    if (int.TryParse(spiltProxies[1], out int prefixLength) &&
                        IPAddress.TryParse(spiltProxies[0], out IPAddress prefixIpAddress))
                    {
                        options.KnownNetworks.Add(new IPNetwork(prefixIpAddress, prefixLength));
                    }
                }
            }
        }

        private static void AddProxies(IList<IPAddress> addresses, IList<string> proxiesAddress)
        {
            if (proxiesAddress.Any())
            {
                addresses.Clear();
                foreach (var proxy in proxiesAddress)
                {
                    if (IPAddress.TryParse(proxy, out IPAddress iPAddress))
                    {
                        addresses.Add(iPAddress);
                    }
                }
            }
        }
    }
}
