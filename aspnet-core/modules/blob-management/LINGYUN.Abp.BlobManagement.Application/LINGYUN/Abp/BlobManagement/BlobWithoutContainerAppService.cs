using LINGYUN.Abp.BlobManagement.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace LINGYUN.Abp.BlobManagement;

public abstract class BlobWithoutContainerAppService : BlobAppServiceBase
{
    protected abstract string ContainerName { get; }

    protected BlobWithoutContainerAppService(
        IBlobContainerRepository blobContainerRepository,
        IBlobRepository blobRepository,
        BlobManager blobManager)
        : base(blobContainerRepository, blobRepository, blobManager)
    {
    }

    public async virtual Task CreateChunkFileAsync(BlobFileChunkCreateWithoutContainerDto input)
    {
        await CheckCreatePolicyAsync();

        await base.CreateChunkFileAsync(ContainerName, input);
    }

    public async virtual Task<BlobDto> CreateFileAsync(BlobFileCreateWithoutContainerDto input)
    {
        await CheckCreatePolicyAsync();

        var blobContainer = await GetOrCreateContainer();

        return await base.CreateFileAsync(blobContainer, input);
    }

    public async virtual Task<BlobDto> CreateFolderAsync(BlobFolderCreateWithoutContainerDto input)
    {
        await CheckCreatePolicyAsync();

        var blobContainer = await GetOrCreateContainer();

        return await base.CreateFolderAsync(blobContainer, input);
    }

    public async virtual Task<IRemoteStreamContent> DownloadByNameAsync(string name)
    {
        var blobContainer = await BlobContainerRepository.GetByNameAsync(BlobManager.GetBlobProvider(), ContainerName);

        return await base.DownloadByNameAsync(blobContainer, name);
    }

    public async virtual Task<PagedResultDto<BlobDto>> GetListAsync(BlobGetPagedListWithoutContainerInput input)
    {
        var blobContainer = await GetOrCreateContainer();

        return await base.GetListAsync(new BlobGetPagedListInput
        {
            SkipCount = input.SkipCount,
            MaxResultCount = input.MaxResultCount,
            Sorting = input.Sorting,
            ContentType = input.ContentType,
            ParentId = input.ParentId,
            Type = input.Type,
            ContainerId = blobContainer.Id,
        });
    }

    protected async virtual Task<BlobContainer> GetOrCreateContainer()
    {
        var blobContainer = await BlobManager.FindContainerAsync(ContainerName)
            ?? await BlobManager.CreateContainerAsync(ContainerName);

        return blobContainer;
    }

    protected async override Task CheckGetPolicyAsync(Blob blob)
    {
        await CheckGetPolicyAsync(ContainerName, blob);
    }

    protected async override Task CheckDeletePolicyAsync(Blob blob)
    {
        await CheckDeletePolicyAsync(ContainerName, blob);
    }
}
