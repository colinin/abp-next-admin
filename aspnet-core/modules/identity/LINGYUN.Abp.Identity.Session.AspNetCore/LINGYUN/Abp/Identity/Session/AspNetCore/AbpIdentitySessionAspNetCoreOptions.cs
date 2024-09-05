using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Identity.Session.AspNetCore;
public class AbpIdentitySessionAspNetCoreOptions
{
    /// <summary>
    /// 是否解析IP地理信息
    /// </summary>
    public bool IsParseIpLocation { get; set; }
    /// <summary>
    /// 不做处理的省份
    /// </summary>
    public IList<string> IgnoreProvinces { get; set; }
    /// <summary>
    /// 地理信息解析
    /// </summary>
    public Func<LocationInfo, string> LocationParser {  get; set; }

    public AbpIdentitySessionAspNetCoreOptions()
    {
        IsParseIpLocation = false;
        IgnoreProvinces = new List<string>
        {
            // 中国直辖市不显示省份数据
            "北京", "上海", "天津", "重庆"
        };
        LocationParser = (locationInfo) =>
        {
            var location = "";

            if (!locationInfo.Province.IsNullOrWhiteSpace() &&
                !IgnoreProvinces.Contains(locationInfo.Province))
            {
                location += locationInfo.Province + " ";
            }
            if (!locationInfo.City.IsNullOrWhiteSpace())
            {
                location += locationInfo.City;
            }
            if (location.IsNullOrWhiteSpace() && !locationInfo.Country.IsNullOrWhiteSpace())
            {
                location = locationInfo.Country;
            }
            return location;
        };
    }
}
