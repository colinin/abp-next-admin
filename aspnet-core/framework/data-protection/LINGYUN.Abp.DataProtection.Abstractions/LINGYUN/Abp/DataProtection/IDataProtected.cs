using System;

namespace LINGYUN.Abp.DataProtection;

public interface IDataProtected
{
    Guid? CreatorId { get; }
}