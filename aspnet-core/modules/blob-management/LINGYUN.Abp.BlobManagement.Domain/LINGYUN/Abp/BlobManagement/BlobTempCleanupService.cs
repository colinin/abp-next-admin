using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.BlobManagement;

public class BlobTempCleanupService : ITransientDependency
{
    public ILogger<BlobTempCleanupService> Logger { get; set; }
    protected AbpBlobManagementOptions Options { get; }
    protected IClock Clock { get; set; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IBlobRepository BlobRepository { get; }
    protected IBlobContainerRepository BlobContainerRepository { get; }
    protected BlobManager BlobManager { get; }

    public BlobTempCleanupService(
        IClock clock,
        ICurrentTenant currentTenant,
        IOptions<AbpBlobManagementOptions> options,
        IBlobContainerRepository blobContainerRepository,
        IBlobRepository blobRepository,
        BlobManager blobManager)
    {
        Clock = clock;
        CurrentTenant = currentTenant;
        Options = options.Value;
        BlobContainerRepository = blobContainerRepository;
        BlobRepository = blobRepository;
        BlobManager = blobManager;

        Logger = NullLogger<BlobTempCleanupService>.Instance;
    }

    public virtual async Task CleanAsync()
    {
        Logger.LogInformation("Start blob cleanup service.");

        if (Options.IsCleanupEnabled)
        {
            var host = CurrentTenant.IsAvailable ? CurrentTenant.Name ?? CurrentTenant.Id.ToString() : "host";

            Logger.LogInformation("Start cleanup {host} temp blobs.", host);

            try
            {
                var tempBlobContainer = await BlobContainerRepository.FindByNameAsync("temp");
                if (tempBlobContainer == null)
                {
                    Logger.LogInformation($"No temp container found.");
                    return;
                }

                var threshold = Clock.Now - Options.MinimumTempLifeSpan;

                var expiredTempFiles = await BlobRepository.GetListAsync(
                    new ExpressionSpecification<Blob>(
                        x => x.ContainerId == tempBlobContainer.Id &&
                             x.Type == BlobType.File &&
                             x.CreationTime <= threshold),
                    sorting: $"{nameof(Blob.CreationTime)} DESC",
                    maxResultCount: Options.MaximumTempSize);

                foreach ( var file in expiredTempFiles)
                {
                    await BlobManager.DeleteBlobAsync(file);
                }

                var expiredTempFolders = await BlobRepository.GetListAsync(
                    new ExpressionSpecification<Blob>(
                        x => x.ContainerId == tempBlobContainer.Id &&
                             x.Type == BlobType.Folder &&
                             x.CreationTime <= threshold),
                    sorting: $"{nameof(Blob.CreationTime)} DESC",
                    maxResultCount: Options.MaximumTempSize);

                foreach (var folder in expiredTempFolders)
                {
                    await BlobManager.DeleteBlobAsync(folder);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Failed to clear expired blobs: {message}", ex.Message);
            }
        }
    }
}
