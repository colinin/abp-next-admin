using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IP.Location;
public abstract class IPLocationResolveContributorBase : IIPLocationResolveContributor
{
    public abstract string Name { get; }

    public abstract Task ResolveAsync(IIPLocationResolveContext context);

    /// <summary>
    /// 由国家/省份/城市构造 <see cref="IPLocation"/>, 并按 <see cref="AbpIPLocationResolveOptions"/> 生成备注(Remarks)
    /// </summary>
    /// <remarks>
    /// 各 IP 位置解析贡献者共用的备注规则:
    /// * 不显示国家(<see cref="AbpIPLocationResolveOptions.UseCountry"/> 返回 false)时:
    ///   省份与城市都存在则使用"省份+城市"(省市同名时仅使用城市, 如 重庆市);
    /// * 显示省份(<see cref="AbpIPLocationResolveOptions.UseProvince"/> 返回 true)时:
    ///   国家与省份都存在则使用"国家+省份"(如 中国香港);
    /// * 其它情况使用国家(如 日本、美国)。
    /// </remarks>
    protected virtual IPLocation CreateIPLocation(
        IIPLocationResolveContext context,
        string? country = null,
        string? province = null,
        string? city = null)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpIPLocationResolveOptions>>().Value;

        var ipLocation = new IPLocation(country, province, city);

        // 36.133.232.0|36.133.239.255|中国|重庆|重庆市|移动
        if (!options.UseCountry(ipLocation) &&
            !string.IsNullOrWhiteSpace(province) &&
            !string.IsNullOrWhiteSpace(city))
        {
            if (province!.Length <= city!.Length &&
                city.StartsWith(province, StringComparison.InvariantCultureIgnoreCase))
            {
                // 重庆市
                ipLocation.Remarks = city;
            }
            // 111.26.31.0|111.26.31.127|中国|吉林省|吉林市|移动
            else
            {
                // 吉林省吉林市
                ipLocation.Remarks = $"{province}{city}";
            }
        }
        // 220.246.0.0|220.246.255.255|中国|香港|0|电讯盈科
        else if (options.UseProvince(ipLocation) &&
            !string.IsNullOrWhiteSpace(country) &&
            !string.IsNullOrWhiteSpace(province))
        {
            // 中国香港
            ipLocation.Remarks = $"{country}{province}";
        }
        // 103.151.173.0|103.151.173.255|日本|东京|东京|IKUUU网络
        else
        {
            // 日本 / 美国(无省份与城市数据时仅使用国家)
            ipLocation.Remarks = country;
        }

        return ipLocation;
    }
}
