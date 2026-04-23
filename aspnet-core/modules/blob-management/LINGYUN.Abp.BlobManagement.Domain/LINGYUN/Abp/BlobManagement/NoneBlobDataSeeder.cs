using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobManagement;

public class NoneBlobDataSeeder : IBlobDataSeeder
{
    public Task SeedAsync(Guid? tenantId = null)
    {
        return Task.CompletedTask;
    }
}
