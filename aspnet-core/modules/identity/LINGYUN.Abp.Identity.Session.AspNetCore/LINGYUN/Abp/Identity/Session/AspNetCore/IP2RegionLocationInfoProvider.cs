using IP2Region.Net.Abstractions;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Identity.Session.AspNetCore;

public class IP2RegionLocationInfoProvider : IIpLocationInfoProvider
{
    protected static readonly LocationInfo _nullCache = null;

    protected ISearcher Searcher { get; }
    public IP2RegionLocationInfoProvider(ISearcher searcher)
    {
        Searcher = searcher;
    }

    public virtual Task<LocationInfo> GetLocationInfoAsync(string ipAddress)
    {
        var region = Searcher.Search(ipAddress);
        if (region.IsNullOrWhiteSpace())
        {
            return Task.FromResult(_nullCache);
        }

        var regions = region.Split('|');
        // |    0    |      1      |  2 |3|  4   |   5  | 6 |
        // 39.128.0.0|39.128.31.255|中国|0|云南省|昆明市|移动
        var locationInfo = new LocationInfo
        {
            Country = regions.Length >= 3 && !string.Equals(regions[2], "0") ? regions[2] : null,
            Province = regions.Length >= 5 && !string.Equals(regions[4], "0") ? regions[4] : null,
            City = regions.Length >= 6 && !string.Equals(regions[5], "0") ? regions[5] : null,
        };
        return Task.FromResult(locationInfo);
    }
}
