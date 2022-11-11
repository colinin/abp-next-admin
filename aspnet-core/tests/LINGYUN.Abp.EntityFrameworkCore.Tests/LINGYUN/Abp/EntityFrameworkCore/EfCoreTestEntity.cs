using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.EntityFrameworkCore;

public class EfCoreTestEntity : Entity<Guid>
{
    public virtual string PropString { get; set; }
    public virtual int? PropInt32 { get; set; }
    public virtual long? PropInt64 { get; set; }
    public virtual DateTime? DateTime { get; set; }
    public EfCoreTestEntity(
        Guid id,
        string propString = null,
        int? propInt32 = null, 
        long? propInt64 = null, 
        DateTime? dateTime = null)
        : base(id)
    {
        PropString = propString;
        PropInt32 = propInt32;
        PropInt64 = propInt64;
        DateTime = dateTime;
    }
}
