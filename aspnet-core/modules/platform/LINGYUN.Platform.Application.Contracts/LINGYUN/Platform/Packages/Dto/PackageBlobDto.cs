using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace LINGYUN.Platform.Packages;

public class PackageBlobDto : CreationAuditedEntityDto<int>, IHasExtraProperties
{
    public string Name { get; set; }
    public string Url { get; set; }
    public long? Size { get; set; }
    public string Summary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string License { get; set; }
    public string Authors { get; set; }
    public string SHA256 { get; set; }
    public string ContentType { get; set; }
    public int DownloadCount { get; set; }
    public ExtraPropertyDictionary ExtraProperties { get; set; }
}
