using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtection;
public interface IDataProtectionRepository<TEntity> : IDataProtectedEnabled
{
    Task<IQueryable<TEntity>> GetQueryableAsync();
}
