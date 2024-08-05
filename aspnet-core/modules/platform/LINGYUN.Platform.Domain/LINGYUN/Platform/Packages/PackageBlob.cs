using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Packages;

public class PackageBlob : CreationAuditedEntity<int>, IHasExtraProperties
{
    public virtual Guid PackageId { get; private set; }
    public virtual Package Package { get; private set; }
    public virtual string Name { get; protected set; }
    public virtual string Url { get; protected set; }
    public virtual long? Size { get; protected set; }
    public virtual string Summary { get; protected set; }
    public virtual DateTime CreatedAt { get; protected set; }
    public virtual DateTime? UpdatedAt { get; protected set; }
    public virtual string License { get; set; }
    public virtual string Authors { get; set; }
    public virtual string ContentType { get; set; }
    public virtual string SHA256 { get; set; }
    public virtual int DownloadCount { get; protected set; }
    public virtual ExtraPropertyDictionary ExtraProperties { get; set; }

    protected PackageBlob()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    internal PackageBlob(
        Guid packageId,
        string name,
        DateTime createdAt,
        DateTime? updatedAt = null,
        long? size = null,
        string summary = null)
    {
        PackageId = packageId;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), PackageBlobConsts.MaxNameLength);
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Size = size;
        Summary = Check.Length(summary, nameof(summary), PackageBlobConsts.MaxSummaryLength);
        DownloadCount = 0;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public void SetUrl(string url)
    {
        Url = Check.NotNullOrWhiteSpace(url, nameof(url), PackageBlobConsts.MaxUrlLength);
    }

    public void Download()
    {
        DownloadCount += 1;
    }
}
