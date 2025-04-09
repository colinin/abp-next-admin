using System;

namespace LINGYUN.Abp.DataProtection;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class DataProtectedAttribute : Attribute
{
    public DataAccessOperation[] Operations { get; }

    public DataProtectedAttribute() : this(
        DataAccessOperation.Read,
        DataAccessOperation.Write, 
        DataAccessOperation.Delete)
    {
    }

    public DataProtectedAttribute(params DataAccessOperation[] operations)
    {
        Operations = operations;
    }
}
