using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobManagement;

public interface IBlobDataSeeder
{
    Task SeedAsync(Guid? tenantId = null);
}
