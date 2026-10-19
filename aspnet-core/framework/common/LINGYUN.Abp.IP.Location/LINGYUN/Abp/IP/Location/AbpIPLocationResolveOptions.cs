using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.IP.Location;
public class AbpIPLocationResolveOptions
{
    [NotNull]
    public List<IIPLocationResolveContributor> IPLocationResolvers { get; }

    /// <summary>
    /// 从备注(Remarks)中移除的国家名称
    /// </summary>
    /// <remarks>
    /// 名称匹配(忽略大小写与首尾空白, 适用于所有国家的 IP)时, 该国家名称不再出现在备注中,
    /// 备注改用省份与城市组合: 省市都存在则"省份+城市"(省市同名时仅城市, 如 重庆市), 仅有省份时使用省份。
    /// 
    /// 例如设置为 "中国" 后: 中国 IP 的备注为"省份+城市"(如 浙江省杭州市), 其它国家仍仅显示国家名称。
    /// 默认空集合(国家名称参与备注组合)。
    /// </remarks>
    [NotNull]
    public string[] RemoveCountry { get; set; }

    /// <summary>
    /// 从备注(Remarks)中移除的省份名称
    /// </summary>
    /// <remarks>
    /// 名称匹配(忽略大小写与首尾空白, 适用于所有国家的 IP)时, 该省份名称不再出现在备注中, 备注仅使用城市(如 杭州市);
    /// 未匹配的 IP 不受影响。默认空集合(省份名称参与备注组合)。
    /// </remarks>
    [NotNull]
    public string[] RemoveProvince { get; set; }

    public AbpIPLocationResolveOptions()
    {
        IPLocationResolvers = new List<IIPLocationResolveContributor>();

        RemoveCountry = Array.Empty<string>();
        RemoveProvince = Array.Empty<string>();
    }
}
