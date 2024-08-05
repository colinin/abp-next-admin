using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Packages;

public class Package : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; protected set; }
    /// <summary>
    /// 版本说明
    /// </summary>
    public virtual string Note { get; protected set; }
    /// <summary>
    /// 版本
    /// </summary>
    public virtual string Version { get; protected set; }
    /// <summary>
    /// 描述
    /// </summary>
    public virtual string Description { get; set; }
    /// <summary>
    /// 强制更新
    /// </summary>
    public virtual bool ForceUpdate { get; set; }

    public virtual string Authors { get; set; }

    public virtual PackageLevel Level { get; set; }

    public virtual ICollection<PackageBlob> Blobs { get; protected set; }

    protected Package()
    {
        Blobs = new Collection<PackageBlob>();
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public Package(
        Guid id,
        string name,
        string note, 
        string version,
        string description = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), PackageConsts.MaxNameLength);
        Note = Check.NotNullOrWhiteSpace(note, nameof(note), PackageConsts.MaxNoteLength);
        Version = Check.NotNullOrWhiteSpace(version, nameof(version), PackageConsts.MaxVersionLength);

        Description = Check.Length(description, nameof(description), PackageConsts.MaxDescriptionLength);

        Level = PackageLevel.None;
        Blobs = new Collection<PackageBlob>();
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public void SetNote(string note)
    {
        Note = Check.NotNullOrWhiteSpace(note, nameof(note), PackageConsts.MaxNoteLength);
    }

    public PackageBlob CreateBlob(
        string name,
        DateTime createdAt,
        DateTime? updatedAt = null,
        long? size = null,
        string summary = null)
    {
        var findBlob = FindBlob(name);
        if (findBlob == null)
        {
            findBlob = new PackageBlob(
                Id,
                name,
                createdAt,
                updatedAt,
                size,
                summary);
            Blobs.Add(findBlob);
        }
        return findBlob;
    }

    public PackageBlob FindBlob(string name)
    {
        return Blobs.FirstOrDefault(x => x.Name == name);
    }

    public void RemoveBlob(string name)
    {
        Blobs.RemoveAll(x => x.Name == name);
    }

    public void ClearBlob()
    {
        Blobs.Clear();
    }
}
