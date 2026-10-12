using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Net;
using Xunit;

namespace LINGYUN.Abp.MaxMindGeoIP2;

/// <summary>
/// 备注(Remarks)组合规则测试
/// </summary>
/// <remarks>
/// 规则由 LINGYUN.Abp.IP.Location 统一处理, 各位置解析模块(IP2Region、MaxMind)共用:
/// * 省份名称匹配 RemoveProvince -> 备注仅使用城市;
/// * 国家名称匹配 RemoveCountry -> 备注使用省份与城市组合(省市同名仅城市);
/// * 都未匹配 -> 备注仅使用国家名称。
/// 名称匹配忽略大小写与首尾空白, 适用于所有国家的 IP 地址解析。
/// </remarks>
public class IPLocationRemarksTest
{
    [Theory]
    // 都未配置: 仅使用国家名称
    [InlineData("223.5.5.5", new string[0], new string[0], "中国", "浙江省", "杭州市", "中国")]
    // 匹配国家名称(中国): 省份与城市组合
    [InlineData("2400:3200::1", new[] { "中国" }, new string[0], "中国", "浙江省", "杭州市", "浙江省杭州市")]
    [InlineData("36.133.108.1", new[] { "中国" }, new string[0], "中国", "重庆", "重庆市", "重庆市")]
    [InlineData("220.246.0.1", new[] { "中国" }, new string[0], "中国", "香港特别行政区", null, "香港特别行政区")]
    [InlineData("114.114.114.114", new[] { "中国" }, new string[0], "中国", null, null, null)]
    // 未匹配国家名称: 不受影响
    [InlineData("103.151.173.211", new[] { "中国" }, new string[0], "日本", "东京", "东京", "日本")]
    [InlineData("2a00:1450:4001:81b::200e", new[] { "中国" }, new string[0], "德国", "黑森州", "法兰克福", "德国")]
    // 匹配省份名称: 仅使用城市(同样适用于其它国家的 IP)
    [InlineData("111.26.31.1", new string[0], new[] { "吉林省" }, "中国", "吉林省", "吉林市", "吉林市")]
    [InlineData("180.76.76.76", new string[0], new[] { "北京市" }, "中国", "北京市", null, null)]
    [InlineData("2001:200::1", new string[0], new[] { "东京都" }, "日本", "东京都", "新宿区", "新宿区")]
    // 省份优先于国家
    [InlineData("8.8.8.8", new[] { "中国" }, new[] { "浙江省" }, "中国", "浙江省", "杭州市", "杭州市")]
    public void TestRemarks(
        string ip,
        string[] removeCountry,
        string[] removeProvince,
        string country,
        string? province,
        string? city,
        string? expectedRemarks)
    {
        var services = new ServiceCollection();
        services.Configure<AbpIPLocationResolveOptions>(options =>
        {
            options.RemoveCountry = removeCountry;
            options.RemoveProvince = removeProvince;
        });

        var context = new IPLocationResolveContext(IPAddress.Parse(ip), services.BuildServiceProvider());

        var location = new TestIPLocationResolveContributor()
            .Create(context, country, province, city);

        context.IpAddress.ShouldBe(IPAddress.Parse(ip));

        location.Country.ShouldBe(country);
        location.Province.ShouldBe(province);
        location.City.ShouldBe(city);

        if (expectedRemarks == null)
        {
            location.Remarks.ShouldBeNullOrEmpty();
        }
        else
        {
            location.Remarks.ShouldBe(expectedRemarks);
        }
    }

    private sealed class TestIPLocationResolveContributor : MaxMindGeoIP2IPLocationResolveContributor
    {
        public IPLocation Create(
            IIPLocationResolveContext context,
            string? country,
            string? province,
            string? city)
        {
            return CreateIPLocation(context, country, province, city);
        }
    }
}
