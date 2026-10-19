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
    /// 各 IP 位置解析贡献者共用的备注规则(名称匹配忽略大小写与首尾空白, 适用于所有国家的 IP 地址解析):
    /// * 省份名称匹配到 <see cref="AbpIPLocationResolveOptions.RemoveProvince"/> 时, 省份从备注中移除, 备注使用城市(如 杭州市);
    /// * 国家名称匹配到 <see cref="AbpIPLocationResolveOptions.RemoveCountry"/> 时, 国家从备注中移除,
    ///   备注使用省份与城市组合(省市同名时仅使用城市, 如 重庆市; 仅有省份时使用省份, 如 香港特别行政区);
    /// * 都未匹配时, 备注使用国家名称(如 中国、日本、美国);
    /// * 匹配到的名称被移除后没有可显示的层级时, 备注为空。
    /// </remarks>
    protected virtual IPLocation CreateIPLocation(
        IIPLocationResolveContext context,
        string? country = null,
        string? province = null,
        string? city = null)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpIPLocationResolveOptions>>().Value;

        var ipLocation = new IPLocation(country, province, city);

        // 匹配到配置的名称时, 对应的层级从备注中移除
        var useCountry = !IsExcluded(options.RemoveCountry, country);
        var useProvince = !IsExcluded(options.RemoveProvince, province);

        // 220.246.0.0|220.246.255.255|中国|香港|0|电讯盈科
        if (!useProvince)
        {
            // 省份名称匹配到移除列表: 备注使用城市(如 杭州市)
            ipLocation.Remarks = string.IsNullOrWhiteSpace(city) ? null : city;
        }
        // 36.133.232.0|36.133.239.255|中国|重庆|重庆市|移动
        else if (!useCountry)
        {
            // 国家名称匹配到移除列表: 备注使用省份与城市组合
            if (!string.IsNullOrWhiteSpace(province) &&
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
            // 220.246.0.0|220.246.255.255|中国|香港特别行政区|0
            else if (!string.IsNullOrWhiteSpace(province))
            {
                // 香港特别行政区
                ipLocation.Remarks = province;
            }
            else if (!string.IsNullOrWhiteSpace(city))
            {
                ipLocation.Remarks = city;
            }
            else
            {
                // 匹配到的名称已从备注中移除(例如仅有"中国"时备注为空)
                ipLocation.Remarks = null;
            }
        }
        // 103.151.173.0|103.151.173.255|日本|东京|东京|IKUUU网络
        else
        {
            // 日本 / 美国 / 德国(未匹配到移除列表时仅使用国家名称)
            ipLocation.Remarks = country;
        }

        return ipLocation;
    }

    /// <summary>
    /// 名称是否匹配到移除列表(忽略大小写与首尾空白)
    /// </summary>
    protected virtual bool IsExcluded(string[] names, string? name)
    {
        if (names == null || names.Length == 0 || string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        foreach (var item in names)
        {
            if (!string.IsNullOrWhiteSpace(item) &&
                string.Equals(item.Trim(), name!.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
