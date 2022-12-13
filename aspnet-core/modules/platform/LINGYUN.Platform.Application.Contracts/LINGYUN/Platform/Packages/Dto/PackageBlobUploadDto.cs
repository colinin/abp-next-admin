using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Content;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Packages;

public class PackageBlobUploadDto
{
    [Required]
    [DynamicMaxLength(typeof(PackageBlobConsts), nameof(PackageBlobConsts.MaxNameLength))]
    public string Name { get; set; }

    public long? Size { get; set; }

    [DynamicMaxLength(typeof(PackageBlobConsts), nameof(PackageBlobConsts.MaxSummaryLength))]
    public string Summary { get; set; }

    [DynamicMaxLength(typeof(PackageBlobConsts), nameof(PackageBlobConsts.MaxContentTypeLength))]
    public string ContentType { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [DynamicMaxLength(typeof(PackageBlobConsts), nameof(PackageBlobConsts.MaxLicenseLength))]
    public string License { get; set; }

    [DynamicMaxLength(typeof(PackageBlobConsts), nameof(PackageBlobConsts.MaxAuthorsLength))]
    public string Authors { get; set; }

    [Required]
    [DisableAuditing]
    public IRemoteStreamContent File { get; set; }
}
