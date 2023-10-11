using LINGYUN.Abp.Sonatype.Nexus.Assets;
using LINGYUN.Abp.Sonatype.Nexus.Components;
using LINGYUN.Abp.Sonatype.Nexus.Search;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.Nexus;
public class NexusBlobProvider : BlobProviderBase, ITransientDependency
{
    protected INexusAssetManager NexusAssetManager { get; }
    protected INexusComponentManager NexusComponentManager { get; }
    protected INexusLookupService NexusLookupService { get; }
    protected IBlobRawPathCalculator BlobDirectoryCalculator { get; }

    public NexusBlobProvider(
        INexusAssetManager nexusAssetManager,
        INexusComponentManager nexusComponentManager,
        INexusLookupService nexusLookupService,
        IBlobRawPathCalculator blobDirectoryCalculator)
    {
        NexusAssetManager = nexusAssetManager;
        NexusComponentManager = nexusComponentManager;
        NexusLookupService = nexusLookupService;
        BlobDirectoryCalculator = blobDirectoryCalculator;
    }

    public async override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var nexusComponent = await GetNexusomponentOrNull(args);
        if (nexusComponent == null)
        {
            return false;
        }
        return await NexusComponentManager.DeleteAsync(nexusComponent.Id, args.CancellationToken);
    }

    public async override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var nexusAsset = await GetNexusAssetOrNull(args);
        return nexusAsset != null;
    }

    public async override Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var nexusAsset = await GetNexusAssetOrNull(args);
        if (nexusAsset == null)
        {
            return null;
        }

        return await NexusAssetManager.GetContentOrNullAsync(nexusAsset);
    }

    public async override Task SaveAsync(BlobProviderSaveArgs args)
    {
        var nexusAsset = await GetNexusAssetOrNull(args);
        if (!args.OverrideExisting && nexusAsset != null)
        {
            throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
        }

        var fileBytes = await args.BlobStream.GetAllBytesAsync();
        var blobPath = BlobDirectoryCalculator.CalculateGroup(args.ContainerName, args.BlobName);
        var blobName = BlobDirectoryCalculator.CalculateName(args.ContainerName, args.BlobName, true);
        // blobName = blobName.Replace(blobPath.RemovePreFix("/"), "").RemovePreFix("/");
        var asset1 = new Asset(blobName, fileBytes);

        var nexusConfiguration = args.Configuration.GetNexusConfiguration();
        var repository = nexusConfiguration.Repository;
        
        var nexusRawBlobUploadArgs = new NexusRawBlobUploadArgs(
            repository,
            blobPath,
            asset1);

        await NexusComponentManager.UploadAsync(nexusRawBlobUploadArgs, args.CancellationToken);
    }

    protected async virtual Task<NexusAsset> GetNexusAssetOrNull(BlobProviderArgs args)
    {
        var nexusConfiguration = args.Configuration.GetNexusConfiguration();
        var blobPath = BlobDirectoryCalculator.CalculateGroup(args.ContainerName, args.BlobName);
        var blobName = BlobDirectoryCalculator.CalculateName(args.ContainerName, args.BlobName);
        var nexusSearchArgs = new NexusSearchArgs(
           nexusConfiguration.Repository,
           blobPath,
           blobName);

        var nexusAssetListResult = await NexusLookupService.ListAssetAsync(nexusSearchArgs, args.CancellationToken);
        var nexusAsset = nexusAssetListResult.Items.FirstOrDefault();

        return nexusAsset;
    }

    protected async virtual Task<NexusComponent> GetNexusomponentOrNull(BlobProviderArgs args)
    {
        var nexusConfiguration = args.Configuration.GetNexusConfiguration();
        var blobPath = BlobDirectoryCalculator.CalculateGroup(args.ContainerName, args.BlobName);
        var blobName = BlobDirectoryCalculator.CalculateName(args.ContainerName, args.BlobName);
        var nexusSearchArgs = new NexusSearchArgs(
           nexusConfiguration.Repository,
           blobPath,
           blobName);

        var nexusComponentResult = await NexusLookupService.ListComponentAsync(nexusSearchArgs, args.CancellationToken);
        var nexusComponent = nexusComponentResult.Items.FirstOrDefault();

        return nexusComponent;
    }
}
