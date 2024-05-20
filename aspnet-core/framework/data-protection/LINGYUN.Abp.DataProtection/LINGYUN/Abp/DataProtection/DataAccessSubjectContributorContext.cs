using System;

namespace LINGYUN.Abp.DataProtection;
public class DataAccessSubjectContributorContext
{
    public string EntityTypeFullName { get; }
    public DataAccessOperation Operation { get; }
    public IServiceProvider ServiceProvider { get; }
    public DataAccessSubjectContributorContext(
        string entityTypeFullName,
        DataAccessOperation operation,
        IServiceProvider serviceProvider)
    {
        EntityTypeFullName = entityTypeFullName;
        Operation = operation;
        ServiceProvider = serviceProvider;
    }
}
