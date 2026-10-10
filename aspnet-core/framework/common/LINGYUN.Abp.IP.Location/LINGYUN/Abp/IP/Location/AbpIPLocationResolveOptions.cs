using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.IP.Location;
public class AbpIPLocationResolveOptions
{
    [NotNull]
    public List<IIPLocationResolveContributor> IPLocationResolvers { get; }

    /// <summary>
    /// 是否使用国家名称作为备注(Remarks)
    /// </summary>
    /// <remarks>
    /// 默认 true; 例如"仅中国IP不显示国家"可在国家为中国时返回 false
    /// </remarks>
    public Func<IPLocation, bool> UseCountry { get; set; }

    /// <summary>
    /// 是否使用省份名称作为备注(Remarks)
    /// </summary>
    /// <remarks>
    /// 默认 true; 例如"仅中国IP显示省份"可仅在国家为中国时返回 true
    /// </remarks>
    public Func<IPLocation, bool> UseProvince { get; set; }

    public AbpIPLocationResolveOptions()
    {
        IPLocationResolvers = new List<IIPLocationResolveContributor>();

        UseCountry = _ => true;
        UseProvince = _ => true;
    }
}
