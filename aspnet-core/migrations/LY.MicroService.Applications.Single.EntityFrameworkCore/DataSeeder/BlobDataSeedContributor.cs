using LINGYUN.Abp.BlobManagement;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.DataSeeder;

public class BlobDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    protected IBlobDataSeeder BlobDataSeeder { get; }
    public BlobDataSeedContributor(IBlobDataSeeder blobDataSeeder)
    {
        BlobDataSeeder = blobDataSeeder;
    }

    public async virtual Task SeedAsync(DataSeedContext context)
    {
        await BlobDataSeeder.SeedAsync(context.TenantId);
    }
}
