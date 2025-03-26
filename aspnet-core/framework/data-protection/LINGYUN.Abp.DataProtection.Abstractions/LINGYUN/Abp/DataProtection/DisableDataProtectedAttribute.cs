using System;

namespace LINGYUN.Abp.DataProtection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false)]
public class DisableDataProtectedAttribute : Attribute
{
    public DataAccessOperation? Operation { get; }
    public DisableDataProtectedAttribute()
    {
    }

    public DisableDataProtectedAttribute(DataAccessOperation operation)
    {
        Operation = operation;
    }
}
