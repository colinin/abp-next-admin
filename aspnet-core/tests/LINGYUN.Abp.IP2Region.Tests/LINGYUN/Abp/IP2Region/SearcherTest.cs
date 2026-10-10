using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;
using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace LINGYUN.Abp.IP2Region;
public class SearcherTest : AbpIP2RegionTestBase
{
    private const string IPv4XdbFile = "/LINGYUN/Abp/IP2Region/Resources/ip2region_v4.xdb";
    private const string IPv6XdbFile = "/LINGYUN/Abp/IP2Region/Resources/ip2region_v6.xdb";

    /// <summary>
    /// IPv6 离线库数据量较大(约 67 万段), 基准测试按该步长抽样校验
    /// </summary>
    private const int IPv6BenchSampleStep = 512;

    private readonly Stream _xdbStream;

    public SearcherTest()
    {
        var virtualFileProvider = GetRequiredService<IVirtualFileProvider>();
        _xdbStream = virtualFileProvider.GetFileInfo(IPv4XdbFile).CreateReadStream();
    }

    private Stream CreateXdbStream(string fileName)
    {
        var virtualFileProvider = GetRequiredService<IVirtualFileProvider>();
        return virtualFileProvider.GetFileInfo(fileName).CreateReadStream();
    }

    [Theory]
    [InlineData("8.8.8.8", "United States")]
    [InlineData("36.133.108.1", "重庆市")]
    [InlineData("111.26.31.1", "吉林省吉林市")]
    [InlineData("220.246.0.1", "中国香港特别行政区")]
    [InlineData("103.151.173.211", "Japan")]
    [InlineData("103.151.191.5", "Indonesia")]
    public async Task TestSearchLocation(string ip, string shouldBeRemarks)
    {
        var resolver = GetRequiredService<IIPLocationResolver>();
        var result = await resolver.ResolveAsync(ip);
        result.Location.Remarks.ShouldBe(shouldBeRemarks);
    }

    [Theory]
    [InlineData("2001:4860:4860::8888", "United States")]
    [InlineData("2606:4700:4700::1111", "United Kingdom")]
    [InlineData("2400:3200::1", "浙江省杭州市")]
    [InlineData("2001:200::1", "Japan")]
    [InlineData("240e:4c:4008::1", "北京市")]
    [InlineData("2408:8000::1", "北京市")]
    public async Task TestSearchLocationIPv6(string ip, string shouldBeRemarks)
    {
        var resolver = GetRequiredService<IIPLocationResolver>();
        var result = await resolver.ResolveAsync(ip);
        result.Location.Remarks.ShouldBe(shouldBeRemarks);
    }

    [Theory]
    [InlineData("114.114.114.114", "中国|江苏省|南京市|0|CN")]
    [InlineData("119.29.29.29", "中国|北京市|北京市|腾讯|CN")]
    [InlineData("223.5.5.5", "中国|浙江省|杭州市|阿里|CN")]
    [InlineData("180.76.76.76", "中国|北京市|北京市|百度|CN")]
    [InlineData("8.8.8.8", "United States|California|0|Google LLC|US")]
    public void TestSearchCacheContent(string ip, string shouldBeRegion)
    {
        var contentSearcher = new AbpSearcher(CachePolicy.Content, _xdbStream);
        var region = contentSearcher.Search(ip);
        region.ShouldBe(shouldBeRegion);
    }

    [Theory]
    [InlineData("114.114.114.114", "中国|江苏省|南京市|0|CN")]
    [InlineData("119.29.29.29", "中国|北京市|北京市|腾讯|CN")]
    [InlineData("223.5.5.5", "中国|浙江省|杭州市|阿里|CN")]
    [InlineData("180.76.76.76", "中国|北京市|北京市|百度|CN")]
    [InlineData("8.8.8.8", "United States|California|0|Google LLC|US")]
    public void TestSearchCacheVector(string ip, string shouldBeRegion)
    {
        var vectorSearcher = new AbpSearcher(CachePolicy.VectorIndex, _xdbStream);
        var region = vectorSearcher.Search(ip);
        region.ShouldBe(shouldBeRegion);
    }

    [Theory]
    [InlineData("114.114.114.114", "中国|江苏省|南京市|0|CN")]
    [InlineData("119.29.29.29", "中国|北京市|北京市|腾讯|CN")]
    [InlineData("223.5.5.5", "中国|浙江省|杭州市|阿里|CN")]
    [InlineData("180.76.76.76", "中国|北京市|北京市|百度|CN")]
    [InlineData("8.8.8.8", "United States|California|0|Google LLC|US")]
    public void TestSearchCacheFile(string ip, string shouldBeRegion)
    {
        var fileSearcher = new AbpSearcher(CachePolicy.File, _xdbStream);
        var region = fileSearcher.Search(ip);
        region.ShouldBe(shouldBeRegion);
    }

    [Theory]
    [InlineData("2001:4860:4860::8888", "United States|Florida|Miami|Google LLC|US")]
    [InlineData("2606:4700:4700::1111", "United Kingdom|England|London|Cloudflare, Inc.|GB")]
    [InlineData("2400:3200::1", "中国|浙江省|杭州市|阿里|CN")]
    [InlineData("2001:200::1", "Japan|Tokyo|Tokyo|WIDE Project|JP")]
    [InlineData("240e:4c:4008::1", "中国|北京市|北京市|电信|CN")]
    [InlineData("2a00:1450:4001:81b::200e", "Germany|Hesse|Frankfurt am Main|Google LLC|DE")]
    public void TestSearchIPv6CacheContent(string ip, string shouldBeRegion)
    {
        using var ipv6XdbStream = CreateXdbStream(IPv6XdbFile);
        // 单文件模式: 只加载 IPv6 离线库
        using var searcher = new AbpSearcher(CachePolicy.Content, ipv6XdbStream);

        searcher.Search(ip).ShouldBe(shouldBeRegion);
    }

    [Theory]
    [InlineData("2001:4860:4860::8888", "United States|Florida|Miami|Google LLC|US")]
    [InlineData("2606:4700:4700::1111", "United Kingdom|England|London|Cloudflare, Inc.|GB")]
    [InlineData("2400:3200::1", "中国|浙江省|杭州市|阿里|CN")]
    [InlineData("2001:200::1", "Japan|Tokyo|Tokyo|WIDE Project|JP")]
    [InlineData("240e:4c:4008::1", "中国|北京市|北京市|电信|CN")]
    [InlineData("2a00:1450:4001:81b::200e", "Germany|Hesse|Frankfurt am Main|Google LLC|DE")]
    public void TestSearchIPv6CacheVector(string ip, string shouldBeRegion)
    {
        using var ipv6XdbStream = CreateXdbStream(IPv6XdbFile);
        using var searcher = new AbpSearcher(CachePolicy.VectorIndex, ipv6XdbStream);

        searcher.Search(ip).ShouldBe(shouldBeRegion);
    }

    [Theory]
    [InlineData("2001:4860:4860::8888", "United States|Florida|Miami|Google LLC|US")]
    [InlineData("2606:4700:4700::1111", "United Kingdom|England|London|Cloudflare, Inc.|GB")]
    [InlineData("2400:3200::1", "中国|浙江省|杭州市|阿里|CN")]
    [InlineData("2001:200::1", "Japan|Tokyo|Tokyo|WIDE Project|JP")]
    [InlineData("240e:4c:4008::1", "中国|北京市|北京市|电信|CN")]
    [InlineData("2a00:1450:4001:81b::200e", "Germany|Hesse|Frankfurt am Main|Google LLC|DE")]
    public void TestSearchIPv6CacheFile(string ip, string shouldBeRegion)
    {
        using var ipv6XdbStream = CreateXdbStream(IPv6XdbFile);
        using var searcher = new AbpSearcher(CachePolicy.File, ipv6XdbStream);

        searcher.Search(ip).ShouldBe(shouldBeRegion);
    }

    /// <summary>
    /// 同时加载 IPv4 与 IPv6 离线库时, 按地址族自动路由
    /// </summary>
    [Theory]
    [InlineData("114.114.114.114", "中国|江苏省|南京市|0|CN")]
    [InlineData("8.8.8.8", "United States|California|0|Google LLC|US")]
    [InlineData("2001:4860:4860::8888", "United States|Florida|Miami|Google LLC|US")]
    [InlineData("2400:3200::1", "中国|浙江省|杭州市|阿里|CN")]
    [InlineData("2001:200::1", "Japan|Tokyo|Tokyo|WIDE Project|JP")]
    public void TestSearchDualDatabase(string ip, string shouldBeRegion)
    {
        using var ipv4XdbStream = CreateXdbStream(IPv4XdbFile);
        using var ipv6XdbStream = CreateXdbStream(IPv6XdbFile);
        using var searcher = new AbpSearcher(CachePolicy.File, ipv4XdbStream, ipv6XdbStream);

        searcher.Search(ip).ShouldBe(shouldBeRegion);
    }

    /// <summary>
    /// IPv4 映射的 IPv6 地址(::ffff:x.x.x.x)会归一到 IPv4 查询
    /// </summary>
    [Theory]
    [InlineData("::ffff:114.114.114.114", "中国|江苏省|南京市|0|CN")]
    [InlineData("::ffff:8.8.8.8", "United States|California|0|Google LLC|US")]
    public void TestSearchIPv4MappedIPv6(string ip, string shouldBeRegion)
    {
        using var ipv4XdbStream = CreateXdbStream(IPv4XdbFile);
        using var ipv6XdbStream = CreateXdbStream(IPv6XdbFile);
        using var searcher = new AbpSearcher(CachePolicy.File, ipv4XdbStream, ipv6XdbStream);

        searcher.Search(ip).ShouldBe(shouldBeRegion);
    }

    /// <summary>
    /// 内网、回环、保留地址等特殊地址不返回国家/省份/城市, Remarks 为空
    /// </summary>
    [Theory]
    [InlineData("10.0.0.1")]
    [InlineData("172.16.0.1")]
    [InlineData("192.168.1.1")]
    [InlineData("169.254.1.1")]
    [InlineData("100.64.0.1")]
    [InlineData("192.0.2.1")]
    [InlineData("224.0.0.1")]
    [InlineData("255.255.255.255")]
    [InlineData("fd00::1")]
    [InlineData("fe80::1")]
    [InlineData("ff02::1")]
    [InlineData("2001:db8::1")]
    [InlineData("64:ff9b::1")]
    public async Task TestSearchSpecialLocation(string ip)
    {
        var resolver = GetRequiredService<IIPLocationResolver>();

        var result = await resolver.ResolveAsync(ip);

        result.Location.ShouldNotBeNull();
        result.Location.Remarks.ShouldBeNullOrEmpty();
        result.Location.Country.ShouldBeNull();
        result.Location.Province.ShouldBeNull();
        result.Location.City.ShouldBeNull();
    }

    /// <summary>
    /// 非法地址、本地回环与未指定地址由 IIPLocationResolver 统一处理, 不返回位置
    /// </summary>
    [Theory]
    [InlineData("not-an-ip")]
    [InlineData("999.999.999.999")]
    [InlineData("127.0.0.1")]
    [InlineData("::1")]
    [InlineData("0.0.0.0")]
    [InlineData("::")]
    public async Task TestSearchWithoutLocation(string ip)
    {
        var resolver = GetRequiredService<IIPLocationResolver>();

        var result = await resolver.ResolveAsync(ip);

        result.Location.ShouldBeNull();
    }

    /// <summary>
    /// 默认选项: 内置离线库(虚拟文件系统路径) + CachePolicy.Content(并发安全)
    /// </summary>
    [Fact]
    public void TestDefaultOptions()
    {
        var options = GetRequiredService<IOptions<AbpIP2RegionOptions>>().Value;

        options.IPv4DatabaseFile.ShouldBe(AbpIP2RegionModule.DefaultIPv4DatabaseFile);
        options.IPv6DatabaseFile.ShouldBe(AbpIP2RegionModule.DefaultIPv6DatabaseFile);
        options.CachePolicy.ShouldBe(CachePolicy.Content);
    }

    /// <summary>
    /// 单例查询器并发查询 IPv4 / IPv6 时应返回正确结果(缓存策略需保证并发安全)
    /// </summary>
    [Fact]
    public async Task TestSearchConcurrently()
    {
        var searcher = GetRequiredService<ISearcher>();

        var expected = new (string Ip, string Region)[]
        {
            ("114.114.114.114", "中国|江苏省|南京市|0|CN"),
            ("8.8.8.8", "United States|California|0|Google LLC|US"),
            ("223.5.5.5", "中国|浙江省|杭州市|阿里|CN"),
            ("2001:4860:4860::8888", "United States|Florida|Miami|Google LLC|US"),
            ("2400:3200::1", "中国|浙江省|杭州市|阿里|CN"),
        };

        var tasks = Enumerable.Range(0, Environment.ProcessorCount * 8)
            .Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < 50; i++)
                {
                    foreach (var (ip, region) in expected)
                    {
                        searcher.Search(ip).ShouldBe(region);
                    }
                }
            }));

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// 未提供 IPv6 离线库时, IPv6 查询返回 null(不使用 IPv4 离线库解析), IPv4 查询不受影响
    /// </summary>
    [Fact]
    public void TestSearchIPv6WithoutDatabase()
    {
        using var ipv4XdbStream = CreateXdbStream(IPv4XdbFile);
        using var searcher = new AbpSearcher(CachePolicy.File, ipv4XdbStream, null);

        searcher.Search("2001:4860:4860::8888").ShouldBeNull();
        searcher.Search("114.114.114.114").ShouldBe("中国|江苏省|南京市|0|CN");
    }

    /// <summary>
    /// IPv6 离线库查询结果应与官方源数据一致(抽样校验)
    /// </summary>
    [Theory]
    [InlineData(CachePolicy.VectorIndex)]
    [InlineData(CachePolicy.File)]
    public void TestBenchSearchIPv6(CachePolicy cachePolicy)
    {
        using var ipv6XdbStream = CreateXdbStream(IPv6XdbFile);
        using var searcher = new AbpSearcher(cachePolicy, ipv6XdbStream);

        var srcPath = Path.Combine(AppContext.BaseDirectory, "ipv6_source.txt");
        var lineIndex = 0;

        foreach (var line in File.ReadLines(srcPath))
        {
            // 数据量较大, 按步长抽样
            if (lineIndex++ % IPv6BenchSampleStep != 0)
            {
                continue;
            }

            var ps = line.Trim().Split("|", 3);

            if (ps.Length != 3)
            {
                throw new ArgumentException($"invalid ip segment line {line}", nameof(line));
            }

            var sip = IpAddressToBigInteger(ps[0]);
            var eip = IpAddressToBigInteger(ps[1]);
            var mip = (sip + eip) / 2;

            BigInteger[] temp = [sip, (sip + mip) / 2, mip, (mip + eip) / 2, eip];

            foreach (var ip in temp)
            {
                var ipAddress = BigIntegerToIpAddress(ip);
                var region = searcher.Search(ipAddress);

                if (region != ps[2])
                {
                    throw new Exception($"failed search {ipAddress} with ({region}!={ps[2]})");
                }
            }
        }
    }

    [Theory]
    [InlineData(CachePolicy.Content)]
    [InlineData(CachePolicy.VectorIndex)]
    [InlineData(CachePolicy.File)]
    public void TestBenchSearch(CachePolicy cachePolicy)
    {
        var searcher = new AbpSearcher(cachePolicy, _xdbStream);
        var srcPath = Path.Combine(AppContext.BaseDirectory, "ipv4_source.txt");

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

    private static BigInteger IpAddressToBigInteger(string ip)
        => new(IPAddress.Parse(ip).GetAddressBytes(), isUnsigned: true, isBigEndian: true);

    private static IPAddress BigIntegerToIpAddress(BigInteger value)
    {
        var bytes = value.ToByteArray(isUnsigned: true, isBigEndian: true);
        if (bytes.Length == 16)
        {
            return new IPAddress(bytes);
        }

        var padded = new byte[16];
        Array.Copy(bytes, 0, padded, 16 - bytes.Length, bytes.Length);

        return new IPAddress(padded);
    }
}
