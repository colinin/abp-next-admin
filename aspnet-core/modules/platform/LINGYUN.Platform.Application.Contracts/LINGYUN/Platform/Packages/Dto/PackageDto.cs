using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Platform.Packages;

public class PackageDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 版本说明
    /// </summary>
    public string Note { get; set; }
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 强制更新
    /// </summary>
    public bool ForceUpdate { get; set; }

    public string Authors { get; set; }

    public PackageLevel Level { get; set; }

    public List<PackageBlobDto> Blobs { get; set; } = new List<PackageBlobDto>();

    public static PackageDto None()
    {
        return new PackageDto
        {
            Level = PackageLevel.None,
            Blobs = new List<PackageBlobDto>()
        };
    }
}
