namespace LINGYUN.Abp.DataProtection;
public interface IDataProtectedResourceStore
{
    void Set(DataAccessResource resource);

    void Remove(DataAccessResource resource);

    DataAccessResource Get(string subjectName, string subjectId, string entityTypeFullName, DataAccessOperation operation);
}
