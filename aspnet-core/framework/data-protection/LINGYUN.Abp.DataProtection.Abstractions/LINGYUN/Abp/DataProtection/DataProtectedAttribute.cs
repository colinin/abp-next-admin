using System;

namespace LINGYUN.Abp.DataProtection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DataProtectedAttribute : Attribute
{
    public DataAccessOperation Operation { get; }
    public DataProtectedAttribute() : this(DataAccessOperation.Read)
    {
    }

    public DataProtectedAttribute(DataAccessOperation operation)
    {
        Operation = operation;
    }
}
