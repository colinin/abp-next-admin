using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement;

public class BlobDataSeeder : IBlobDataSeeder, ITransientDependency
{
    public ILogger<BlobDataSeeder> Logger { protected get; set; }

    protected IBlobContainerRepository BlobContainerRepository { get; }
    protected BlobManager BlobManager { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IOptions<AbpBlobManagementOptions> BlobOptions { get; }

    public BlobDataSeeder(
        IBlobContainerRepository blobContainerRepository, 
        BlobManager blobManager, 
        ICurrentTenant currentTenant, 
        IOptions<AbpBlobManagementOptions> blobOptions)
    {
        BlobContainerRepository = blobContainerRepository;
        BlobManager = blobManager;
        CurrentTenant = currentTenant;
        BlobOptions = blobOptions;

        Logger = NullLogger<BlobDataSeeder>.Instance;
    }

    public async virtual Task SeedAsync(Guid? tenantId = null)
    {
        using (CurrentTenant.Change(tenantId))
        {
            Logger.LogInformation("Seeding the static blob containers...");

            foreach (var containerName in BlobOptions.Value.StaticContainers)
            {
                Logger.LogInformation("Seeding container {containerName}...", containerName);

                if (await BlobContainerRepository.FindByNameAsync(containerName) == null)
                {
                    await BlobManager.CreateContainerAsync(containerName);
                }
            }

            Logger.LogInformation("Seeding the static blob containers completed.");
        }
    }
}
