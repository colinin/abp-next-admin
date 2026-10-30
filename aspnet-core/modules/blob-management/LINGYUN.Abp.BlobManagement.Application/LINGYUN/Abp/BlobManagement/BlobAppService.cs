using LINGYUN.Abp.BlobManagement.Dtos;
using LINGYUN.Abp.BlobManagement.Features;
using LINGYUN.Abp.BlobManagement.Permissions;
using System.Threading.Tasks;
using Volo.Abp.Content;
using Volo.Abp.Features;

namespace LINGYUN.Abp.BlobManagement;

[RequiresFeature(BlobManagementFeatureNames.Blob.Enable)]
public class BlobAppService : BlobAppServiceBase, IBlobAppService
{
    public BlobAppService(
        IBlobContainerRepository blobContainerRepository,
        IBlobRepository blobRepository,
        BlobManager blobManager)
        : base(blobContainerRepository, blobRepository, blobManager)
    {
        CreatePolicyName = BlobManagementPermissionNames.Blob.Create;
        DeletePolicyName = BlobManagementPermissionNames.Blob.Delete;
        GetPolicyName = BlobManagementPermissionNames.Blob.Default;
        GetListPolicyName = BlobManagementPermissionNames.Blob.Default;
    }

    public async virtual Task CreateChunkFileAsync(BlobFileChunkCreateDto input)
    {
        await CheckCreatePolicyAsync();

        await base.CreateChunkFileAsync(input.ContainerId.ToString(), input);
    }

    public async virtual Task<BlobDto> CreateFileAsync(BlobFileCreateDto input)
    {
        await CheckCreatePolicyAsync();

        var blobContainer = await BlobContainerRepository.GetAsync(input.ContainerId);

        return await base.CreateFileAsync(blobContainer, input);
    }

    public async virtual Task<BlobDto> CreateFolderAsync(BlobFolderCreateDto input)
    {
        await CheckCreatePolicyAsync();

        var blobContainer = await BlobContainerRepository.GetAsync(input.ContainerId);

        return await base.CreateFolderAsync(blobContainer, input);
    }

    public async virtual Task<IRemoteStreamContent> DownloadAsync(BlobDownloadByIdInput input)
    {
        return await base.DownloadByKeyAsync(input.Key);
    }

    public async virtual Task<IRemoteStreamContent> PreviewAsync(BlobDownloadByIdInput input)
    {
        return await base.DownloadByKeyAsync(input.Key);
    }

    public async virtual Task<IRemoteStreamContent> DownloadByNameAsync(BlobDownloadByNameInput input)
    {
        // fix: 只能在当前租户上下文中访问, 移除通过参数切换租户造成跨租户越权访问
        var blobContainer = await BlobContainerRepository.GetByNameAsync(BlobManager.GetBlobProvider(), input.ContainerName);

        return await base.DownloadByNameAsync(blobContainer, input.BlobName);
    }

    protected async override Task CheckGetPolicyAsync(Blob blob)
    {
        await base.CheckGetPolicyAsync(blob);

        await CheckContainerGetPolicyAsync(blob);
    }

    protected async override Task CheckGetPolicyAsync(string containerName, Blob blob)
    {
        await base.CheckGetPolicyAsync(blob);

        await base.CheckGetPolicyAsync(containerName, blob);
    }

    protected async override Task CheckDeletePolicyAsync(Blob blob)
    {
        await base.CheckDeletePolicyAsync(blob);

        await CheckContainerDeletePolicyAsync(blob);
    }

    protected async override Task CheckDeletePolicyAsync(string containerName, Blob blob)
    {
        await base.CheckDeletePolicyAsync(blob);

        await base.CheckDeletePolicyAsync(containerName, blob);
    }

    protected async virtual Task CheckContainerGetPolicyAsync(Blob blob)
    {
        var containerName = await BlobContainerRepository.GetNameAsync(blob.ContainerId);
        await base.CheckGetPolicyAsync(containerName, blob);
    }

    /// <summary>
    protected async virtual Task CheckContainerDeletePolicyAsync(Blob blob)
    {
        var containerName = await BlobContainerRepository.GetNameAsync(blob.ContainerId);
        await base.CheckDeletePolicyAsync(containerName, blob);
    }
}
