using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement;

public class Blob : AuditedAggregateRoot<Guid>, IMultiTenant
{
    private const string MD5Key = "MD5";

    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid ContainerId { get; protected set; }

    public virtual Guid? ParentId { get; protected set; }

    public virtual string Name { get; protected set; }

    public virtual BlobType Type { get; protected set; }

    public virtual string? ContentType { get; protected set; }

    public virtual long Size { get; protected set; }

    public virtual DateTime? ExpirationTime { get; protected set; }

    public virtual string Provider { get; protected set; }

    public virtual string FullName { get; protected set; }

    public virtual long DownloadCount { get; protected set; }

    public virtual ICollection<Blob> Blobs { get; protected set; }
    protected Blob()
    {
        Blobs = new Collection<Blob>();
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    internal Blob(
        Guid id,
        Guid containerId,
        [NotNull] string name,
        BlobType type,
        long? size = null,
        Guid? parentId = null) : base(id)
    {
        ContainerId = containerId;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), BlobConsts.MaxNameLength);
        Type = type;
        Size = size ?? 0L;
        ParentId = parentId;

        Provider = default!;
        DownloadCount = 0L;
        FullName = CalculateBlobFullName("/", Name);

        Blobs = new Collection<Blob>();
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public void SetFullName(string fullName)
    {
        FullName = Check.NotNullOrWhiteSpace(fullName, nameof(fullName), BlobConsts.MaxFullNameLength);
    }

    public void SetProvider(string provider)
    {
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), BlobConsts.MaxProviderLength);
    }

    public void SetFileInfo(string? contentType = null, long? size = null)
    {
        Size = size ?? 0L;
        ContentType = Check.Length(contentType, nameof(contentType), BlobConsts.MaxContentTypeLength);
    }

    public void SetExpire(DateTime? expirationTime = null)
    {
        ExpirationTime = expirationTime;
    }

    public void SetDownloadCount(long downloadCount)
    {
        DownloadCount = downloadCount;
    }

    public Blob AddBlob(
        Guid id,
        [NotNull] string name,
        BlobType type,
        long? size = null)
    {
        if (Type != BlobType.Folder)
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Blob.NonFolderChildBlob,
                "Sub-files/directories cannot be added to non-directory types!");
        }

        if (HasBlob(name))
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Blob.NameAlreadyExists,
                $"There is already a file/directory named {name} in the current directory!")
                .WithData("Name", name);
        }

        var blob = new Blob(
            id,
            ContainerId,
            name,
            type,
            size,
            Id);

        blob.SetFullName(CalculateBlobFullName(FullName, name));

        Blobs.Add(blob);

        CalculateSize();

        return blob;
    }

    public Blob? FindBlob(string name)
    {
        return Blobs.FirstOrDefault(x => x.Name == name);
    }

    public void RemoveBlob(string name)
    {
        var blob = FindBlob(name);
        if (blob == null)
        {
            return;
        }

        Blobs.Remove(blob);

        Size -= blob.Size;
    }

    public bool HasBlob(string name)
    {
        return Blobs.Any(x => x.Name == name);
    }

    public void CalculateSize()
    {
        if (Type == BlobType.Folder)
        {
            Size = Blobs.Sum(x => x.Size);
        }
    }

    public void SetContentMd5(string md5)
    {
        this.SetProperty(MD5Key, md5);
    }

    public bool CheckMd5(string md5)
    {
        var md5Prop = this.GetProperty(MD5Key, "");

        return string.Equals(md5Prop, md5, StringComparison.InvariantCultureIgnoreCase);
    }

    public static string CalculateBlobFullName(string blobPath, string blobName = "")
    {
        if (blobName.IsNullOrWhiteSpace())
        {
            return blobPath;
        }

        return blobPath.EnsureEndsWith('/') + blobName.RemovePreFix("/");
    }
}
