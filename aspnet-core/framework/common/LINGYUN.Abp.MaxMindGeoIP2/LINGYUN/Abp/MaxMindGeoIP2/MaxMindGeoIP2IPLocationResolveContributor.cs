using LINGYUN.Abp.IP.Location;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MaxMindGeoIP2;

/// <summary>
/// MaxMind GeoIP2 位置解析贡献者
/// </summary>
/// <remarks>
public class MaxMindGeoIP2IPLocationResolveContributor : IPLocationResolveContributorBase
{
    public const string ContributorName = "MaxMindGeoIP2";
    public override string Name => ContributorName;

    public override Task ResolveAsync(IIPLocationResolveContext context)
    {
        var reader = context.ServiceProvider.GetRequiredService<IGeoIP2DatabaseReader>();

        CityResponse? response;
        try
        {
            if (!reader.TryCity(context.IpAddress, out response) || response == null)
            {
                // 内网、保留地址以及未收录的地址在离线库中没有记录
                context.Location = new IPLocation();

                return Task.CompletedTask;
            }
        }
        catch (Exception exception)
        {
            // 离线库读取异常(例如文件损坏)不阻断业务, 记录日志后按无数据返回
            context.ServiceProvider
                .GetService<ILogger<MaxMindGeoIP2IPLocationResolveContributor>>()
                ?.LogWarning(exception, "Failed to query the MaxMind GeoIP2 database for {IpAddress}.", context.IpAddress);

            context.Location = new IPLocation();

            return Task.CompletedTask;
        }

        context.Location = CreateIPLocation(
            context,
            GetName(response.Country),
            GetName(response.MostSpecificSubdivision),
            GetName(response.City));

        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取当前文化的名称, 没有对应文化时回退到 <see cref="NamedEntity.Name"/>
    /// </summary>
    protected virtual string? GetName(NamedEntity? entity)
    {
        if (entity == null)
        {
            return null;
        }

        var culture = CultureInfo.CurrentUICulture;

        // 先按完整文化名称匹配(如 zh-CN), 再按语言名称匹配(如 zh), 最后回退到离线库默认名称
        var name = GetNameByCulture(entity, culture.Name)
            ?? GetNameByCulture(entity, culture.TwoLetterISOLanguageName)
            ?? entity.Name;

        return Normalize(name);
    }

    /// <summary>
    /// 从 <see cref="NamedEntity.Names"/> 中获取指定文化的名称(键大小写不敏感)
    /// </summary>
    protected virtual string? GetNameByCulture(NamedEntity entity, string? culture)
    {
        if (culture.IsNullOrWhiteSpace() || entity.Names == null)
        {
            return null;
        }

        if (entity.Names.TryGetValue(culture!, out var name))
        {
            return name;
        }

        foreach (var item in entity.Names)
        {
            if (string.Equals(item.Key, culture, StringComparison.OrdinalIgnoreCase))
            {
                return item.Value;
            }
        }

        return null;
    }

    private static string? Normalize(string? value)
    {
        return value.IsNullOrWhiteSpace() || string.Equals(value, "0", StringComparison.Ordinal)
            ? null
            : value;
    }
}
