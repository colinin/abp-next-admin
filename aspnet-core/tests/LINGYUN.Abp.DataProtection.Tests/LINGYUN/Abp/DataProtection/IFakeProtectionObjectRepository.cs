using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.DataProtection
{
    public interface IFakeProtectionObjectRepository : IRepository<FakeProtectionObject, int>
    {
        Task<List<FakeProtectionObject>> GetAllListAsync();
    }
}
