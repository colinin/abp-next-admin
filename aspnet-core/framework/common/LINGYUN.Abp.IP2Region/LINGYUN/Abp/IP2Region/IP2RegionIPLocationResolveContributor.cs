using IP2Region.Net.Abstractions;
using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IP2Region;
public class IP2RegionIPLocationResolveContributor : IPLocationResolveContributorBase
{
    public const string ContributorName = "IP2Region";
    public override string Name => ContributorName;

    public override Task ResolveAsync(IIPLocationResolveContext context)
    {
        var searcher = context.ServiceProvider.GetRequiredService<ISearcher>();
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpIP2RegionLocationResolveOptions>>();

        var region = searcher.Search(context.IpAddress);

        if (string.IsNullOrWhiteSpace(region))
        {
            return Task.CompletedTask;
        }

        var regions = region!.Split('|');
        // |    0    |      1      |  2 |  3   |   4  | 5 |
        // 39.128.0.0|39.128.31.255|中国|云南省|昆明市|移动
        // regions:
        // 中国|云南省|昆明市|移动

        var ipLocation = new IPLocation(
            regions.Length >= 1 && !string.Equals(regions[0], "0") ? regions[0] : null,
            regions.Length >= 2 && !string.Equals(regions[1], "0") ? regions[1] : null,
            regions.Length >= 3 && !string.Equals(regions[2], "0") ? regions[2] : null);

        // 36.133.232.0|36.133.239.255|中国|重庆|重庆市|移动
        if (!options.Value.UseCountry(ipLocation) &&
            !ipLocation.Province.IsNullOrWhiteSpace() && 
            !ipLocation.City.IsNullOrWhiteSpace())
        {
            if (ipLocation.Province.Length <= ipLocation.City.Length &&
                ipLocation.City.StartsWith(ipLocation.Province, StringComparison.InvariantCultureIgnoreCase))
            {
                // 重庆市
                ipLocation.Remarks = $"{ipLocation.City}";
            }
            // 111.26.31.0|111.26.31.127|中国|吉林省|吉林市|移动
            else
            {
                // 吉林省吉林市
                ipLocation.Remarks = $"{ipLocation.Province}{ipLocation.City}";
            }
        }
        // 220.246.0.0|220.246.255.255|中国|香港|0|电讯盈科
        else if (options.Value.UseProvince(ipLocation) &&
            !ipLocation.Country.IsNullOrWhiteSpace() && 
            !ipLocation.Province.IsNullOrWhiteSpace())
        {
            // 中国香港
            ipLocation.Remarks = $"{ipLocation.Country}{ipLocation.Province}";
        }
        // 103.151.173.0|103.151.173.255|日本|东京|东京|IKUUU网络
        else
        {
            // 日本
            ipLocation.Remarks = $"{ipLocation.Country}";
        }

        context.Location = ipLocation;

        return Task.CompletedTask;
    }
}
