using System.Threading.Tasks;

namespace LINGYUN.Abp.Saas.Tenants;

public interface IDataBaseConnectionStringChecker
{
    Task<DataBaseConnectionStringCheckResult> CheckAsync(string connectionString);
}
