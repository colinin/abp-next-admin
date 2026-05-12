using LINGYUN.Abp.BlobManagement.Dtos;
using LINGYUN.Abp.BlobManagement.Features;
using LINGYUN.Abp.BlobManagement.Permissions;
using LINGYUN.Abp.Features.LimitValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Features;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.BlobManagement;

public abstract class BlobAppServiceBase : BlobManagementApplicationService
{
    protected virtual string? GetPolicyName { get; set; }

    protected virtual string? GetListPolicyName { get; set; }

    protected virtual string? CreatePolicyName { get; set; }

    protected virtual string? DeletePolicyName { get; set; }

    protected IBlobContainerRepository BlobContainerRepository { get; }
    protected IBlobRepository BlobRepository { get; }
    protected BlobManager BlobManager { get; }

    protected BlobAppServiceBase(
        IBlobContainerRepository blobContainerRepository,
        IBlobRepository blobRepository,
        BlobManager blobManager)
    {
        BlobContainerRepository = blobContainerRepository;
        BlobRepository = blobRepository;
        BlobManager = blobManager;
    }

    public async virtual Task DeleteAsync(Guid id)
    {
        var blob = await BlobRepository.GetAsync(id);

        await DeleteBlobAsync(blob);
    }

    [RequiresFeature(BlobManagementFeatureNames.Blob.DownloadFile)]
    [RequiresLimitFeature(
        BlobManagementFeatureNames.Blob.DownloadLimit,
        BlobManagementFeatureNames.Blob.DownloadInterval,
        LimitPolicy.Month)]
    public async virtual Task<IRemoteStreamContent> GetContentAsync(Guid id)
    {
        var blob = await BlobRepository.GetAsync(id);

        await CheckGetPolicyAsync(blob);

        var stream = await BlobManager.DownloadBlobsync(blob);

        return new RemoteStreamContent(stream ?? Stream.Null, blob.Name, blob.ContentType, blob.Size);
    }

    public async virtual Task<BlobDto> GetAsync(Guid id)
    {
        var blob = await BlobRepository.GetAsync(id);

        await CheckGetPolicyAsync(blob);

        return ObjectMapper.Map<Blob, BlobDto>(blob);
    }

    public async virtual Task<PagedResultDto<BlobDto>> GetListAsync(BlobGetPagedListInput input)
    {
        await CheckGetListPolicyAsync();

        Expression<Func<Blob, bool>> expression = x => x.Provider == BlobManager.GetBlobProvider();

        expression = expression
            .And(x => x.ContainerId == input.ContainerId)
            .And(x => x.ParentId == input.ParentId);

        if (input.Type.HasValue)
        {
            expression = expression.And(x => x.Type == input.Type);
        }
        if (!input.ContentType.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.ContentType == input.ContentType);
        }

        var specification = new ExpressionSpecification<Blob>(expression);

        var totalCount = await BlobRepository.GetCountAsync(specification);
        var blobs = await BlobRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<BlobDto>(totalCount,
            ObjectMapper.Map<List<Blob>, List<BlobDto>>(blobs));
    }

    protected async virtual Task<BlobDto> CreateFileAsync(BlobContainer blobContainer, BlobFileCreateBaseDto input)
    {
        var blobStream = input.File.GetStream();

        Blob? parentBlob = null;
        if (input.ParentId.HasValue)
        {
            parentBlob = await BlobRepository.GetAsync(input.ParentId.Value);
        }

        var blob = await BlobManager.CreateBlobAsync(
            blobContainer,
            input.Name,
            BlobType.File,
            blobStream.Length,
            parentBlob);

        blob = await BlobManager.UploadBlobAsync(blobContainer, blob, blobStream, input.File.ContentType, input.CompareMd5);

        return ObjectMapper.Map<Blob, BlobDto>(blob);
    }

    protected async virtual Task CreateChunkFileAsync(string containerName, BlobFileChunkCreateBaseDto input)
    {
        await BlobManager.CreateChunkBlobAsync(
            containerName,
            input.Name,
            input.File,
            input.ChunkSize,
            input.CurrentChunkSize,
            input.ChunkNumber,
            input.TotalChunks,
            input.CompareMd5,
            input.ParentId);
    }

    protected async virtual Task<BlobDto> CreateFolderAsync(BlobContainer blobContainer, BlobFolderCreateBaseDto input)
    {
        Blob? parentBlob = null;
        if (input.ParentId.HasValue)
        {
            parentBlob = await BlobRepository.GetAsync(input.ParentId.Value);
        }
        var blob = await BlobManager.CreateBlobAsync(
            blobContainer,
            input.Name,
            BlobType.Folder,
            parentBlob: parentBlob);

        return ObjectMapper.Map<Blob, BlobDto>(blob);
    }

    protected async virtual Task<IRemoteStreamContent> GetContentByNameAsync(BlobContainer blobContainer, string name)
    {
        var blob = await FindBlobByNameAsync(blobContainer, name)
            ?? throw new BusinessException(
                BlobManagementErrorCodes.Blob.NameNotFound,
                $"There is no file/directory named {name}!")
                .WithData("Name", name);

        await CheckGetPolicyAsync(blobContainer.Name, blob);

        var stream = await BlobManager.DownloadBlobsync(blob);

        return new RemoteStreamContent(stream ?? Stream.Null, blob.Name, blob.ContentType, blob.Size);
    }

    protected async virtual Task<Blob?> FindBlobByNameAsync(BlobContainer blobContainer, string name)
    {
        var blobName = HttpUtility.UrlDecode(name);

        return await BlobRepository.FindByFullNameAsync(blobName.EnsureStartsWith('/'), blobContainer.Id);
    }

    protected async virtual Task DeleteBlobAsync(Blob blob)
    {
        await CheckDeletePolicyAsync(blob);

        await BlobManager.DeleteBlobAsync(blob);
    }

    protected async virtual Task CheckGetPolicyAsync(Blob blob)
    {
        await CheckPolicyAsync(GetPolicyName);
    }

    protected async virtual Task CheckGetListPolicyAsync()
    {
        await CheckPolicyAsync(GetListPolicyName);
    }

    protected async virtual Task CheckCreatePolicyAsync()
    {
        await CheckPolicyAsync(CreatePolicyName);
    }

    protected async virtual Task CheckDeletePolicyAsync(Blob blob)
    {
        await CheckPolicyAsync(DeletePolicyName);
    }

    protected async virtual Task CheckGetPolicyAsync(string containerName, Blob blob)
    {
        var policyCheckProvider = GetBlobPolicyCheckProvider(containerName);
        if (policyCheckProvider != null)
        {
            await policyCheckProvider.CheckAsync(
                new BlobPolicyCheckContext(
                    LazyServiceProvider,
                    BlobManagementPermissionNames.Blob.Resources.View,
                    blob));
        }
    }

    protected async virtual Task CheckDeletePolicyAsync(string containerName, Blob blob)
    {
        var policyCheckProvider = GetBlobPolicyCheckProvider(containerName);
        if (policyCheckProvider != null)
        {
            await policyCheckProvider.CheckAsync(
                new BlobPolicyCheckContext(
                    LazyServiceProvider,
                    BlobManagementPermissionNames.Blob.Resources.Delete,
                    blob));
        }
    }

    protected virtual IBlobPolicyCheckProvider? GetBlobPolicyCheckProvider(string containerName)
    {
        var options = LazyServiceProvider.GetRequiredService<IOptions<AbpBlobManagementOptions>>().Value;
        if (options.BlobPolicyCheckProviders.TryGetValue(containerName, out var policyCheckProvider))
        {
            return policyCheckProvider;
        }

        return null;
    }
}
