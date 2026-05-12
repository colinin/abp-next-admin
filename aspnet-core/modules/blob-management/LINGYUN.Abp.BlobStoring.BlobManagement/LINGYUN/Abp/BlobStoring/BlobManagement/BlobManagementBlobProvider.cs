using LINGYUN.Abp.BlobManagement.Integration;
using LINGYUN.Abp.BlobManagement.Integration.Dtos;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.BlobManagement;

public class BlobManagementBlobProvider : BlobProviderBase, ITransientDependency
{
    private readonly IBlobIntegrationService _blobService;
    public BlobManagementBlobProvider(IBlobIntegrationService blobService)
    {
        _blobService = blobService;
    }

    public async override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        await _blobService.DeleteAsync(
            new BlobFileGetByNameIntegrationDto
            {
                ContainerName = args.ContainerName,
                BlobName = args.BlobName,
            });

        return true;
    }

    public async override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        return await _blobService.ExistsAsync(
            new BlobFileGetByNameIntegrationDto
            {
                ContainerName = args.ContainerName,
                BlobName = args.BlobName,
            });
    }

    public async override Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var content = await _blobService.GetContentAsync(
            new BlobFileGetByNameIntegrationDto
            {
                ContainerName = args.ContainerName,
                BlobName = args.BlobName,
            });

        var stream = content.GetStream();

        return stream == Stream.Null ? null : stream;
    }

    public async override Task SaveAsync(BlobProviderSaveArgs args)
    {
        await _blobService.CreateAsync(
            new BlobFileCreateIntegrationDto
            {
                ContainerName = args.ContainerName,
                BlobName = args.BlobName,
                Overwrite = args.OverrideExisting,
                File = new RemoteStreamContent(args.BlobStream)
            });
    }
}
