using IP2Region.Net.XDB;
using System;
using System.IO;
using System.Linq;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace LINGYUN.Abp.IP2Region;
public class SearcherTest : AbpIP2RegionTestBase
{
    private readonly Stream _xdbStream;

    public SearcherTest()
    {
        var virtualFileProvider = GetRequiredService<IVirtualFileProvider>();
        _xdbStream = virtualFileProvider.GetFileInfo("/LINGYUN/Abp/IP2Region/Resources/ip2region.xdb").CreateReadStream();
    }

    [Theory]
    [InlineData("114.114.114.114")]
    [InlineData("119.29.29.29")]
    [InlineData("223.5.5.5")]
    [InlineData("180.76.76.76")]
    [InlineData("8.8.8.8")]
    public void TestSearchCacheContent(string ip)
    {
        var contentSearcher = new AbpSearcher(CachePolicy.Content, _xdbStream);
        var region = contentSearcher.Search(ip);
        Console.WriteLine(region);
    }

    [Theory]
    [InlineData("114.114.114.114")]
    [InlineData("119.29.29.29")]
    [InlineData("223.5.5.5")]
    [InlineData("180.76.76.76")]
    [InlineData("8.8.8.8")]
    public void TestSearchCacheVector(string ip)
    {
        var vectorSearcher = new AbpSearcher(CachePolicy.VectorIndex, _xdbStream);
        var region = vectorSearcher.Search(ip);
        Console.WriteLine(region);
    }

    [Theory]
    [InlineData("114.114.114.114")]
    [InlineData("119.29.29.29")]
    [InlineData("223.5.5.5")]
    [InlineData("180.76.76.76")]
    [InlineData("8.8.8.8")]
    public void TestSearchCacheFile(string ip)
    {
        var fileSearcher = new AbpSearcher(CachePolicy.File, _xdbStream);
        var region = fileSearcher.Search(ip);
        Console.WriteLine(region);
    }

    [Theory]
    [InlineData(CachePolicy.Content)]
    [InlineData(CachePolicy.VectorIndex)]
    [InlineData(CachePolicy.File)]
    public void TestBenchSearch(CachePolicy cachePolicy)
    {
        var searcher = new AbpSearcher(cachePolicy, _xdbStream);
        var srcPath = Path.Combine(AppContext.BaseDirectory, "ip.merge.txt");

        foreach (var line in File.ReadLines(srcPath))
        {
            var ps = line.Trim().Split("|", 3);

            if (ps.Length != 3)
            {
                throw new ArgumentException($"invalid ip segment line {line}", nameof(line));
            }

            var sip = Util.IpAddressToUInt32(ps[0]);
            var eip = Util.IpAddressToUInt32(ps[1]);
            var mip = Util.GetMidIp(sip, eip);

            uint[] temp = { sip, Util.GetMidIp(sip, mip), mip, Util.GetMidIp(mip, eip), eip };

            foreach (var ip in temp)
            {
                var region = searcher.Search(ip);

                if (region != ps[2])
                {
                    throw new Exception($"failed search {ip} with ({region}!={ps[2]})");
                }
            }
        }
    }
}
