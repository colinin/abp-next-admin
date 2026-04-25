using LINGYUN.Abp.BlobManagement.Dtos;
using LINGYUN.Abp.BlobManagement.Features;
using LINGYUN.Abp.BlobManagement.Permissions;
using LINGYUN.Abp.Features.LimitValidation;
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

    [RequiresFeature(BlobManagementFeatureNames.Blob.UploadFile)]
    [RequiresLimitFeature(
        BlobManagementFeatureNames.Blob.UploadLimit,
        BlobManagementFeatureNames.Blob.UploadInterval,
        LimitPolicy.Month)]
    public async virtual Task CreateChunkFileAsync(BlobFileChunkCreateDto input)
    {
        await CheckCreatePolicyAsync();

        await base.CreateChunkFileAsync(input.ContainerId.ToString(), input);
    }

    [RequiresFeature(BlobManagementFeatureNames.Blob.UploadFile)]
    [RequiresLimitFeature(
        BlobManagementFeatureNames.Blob.UploadLimit,
        BlobManagementFeatureNames.Blob.UploadInterval,
        LimitPolicy.Month)]
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

    [RequiresFeature(BlobManagementFeatureNames.Blob.DownloadFile)]
    [RequiresLimitFeature(
        BlobManagementFeatureNames.Blob.DownloadLimit,
        BlobManagementFeatureNames.Blob.DownloadInterval,
        LimitPolicy.Month)]
    public async virtual Task<IRemoteStreamContent> GetContentByNameAsync(BlobDownloadByNameInput input)
    {
        var blobContainer = await BlobContainerRepository.GetByNameAsync(input.ContainerName);

        return await base.GetContentByNameAsync(blobContainer, input.BlobName);
    }

    protected async override Task CheckGetPolicyAsync(string containerName, Blob blob)
    {
        await base.CheckGetPolicyAsync(blob);
    }

    protected override Task CheckDeletePolicyAsync(string containerName, Blob blob)
    {
        return base.CheckDeletePolicyAsync(blob);
    }
}
