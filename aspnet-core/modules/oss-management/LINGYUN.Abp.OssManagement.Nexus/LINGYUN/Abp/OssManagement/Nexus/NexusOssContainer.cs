using LINGYUN.Abp.BlobStoring.Nexus;
using LINGYUN.Abp.Sonatype.Nexus.Assets;
using LINGYUN.Abp.Sonatype.Nexus.Components;
using LINGYUN.Abp.Sonatype.Nexus.Search;
using LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI;
using LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI.Assets;
using LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI.Browsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OssManagement.Nexus;

internal class NexusOssContainer : IOssContainer, IOssObjectExpireor
{
    protected ICoreUiServiceProxy CoreUiServiceProxy { get; }
    protected INexusAssetManager NexusAssetManager { get; }
    protected INexusComponentManager NexusComponentManager { get; }
    protected INexusLookupService NexusLookupService { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IBlobRawPathCalculator BlobRawPathCalculator { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

    public NexusOssContainer(
        ICoreUiServiceProxy coreUiServiceProxy, 
        INexusAssetManager nexusAssetManager, 
        INexusComponentManager nexusComponentManager, 
        INexusLookupService nexusLookupService, 
        ICurrentTenant currentTenant, 
        IBlobRawPathCalculator blobRawPathCalculator, 
        IBlobContainerConfigurationProvider configurationProvider)
    {
        CoreUiServiceProxy = coreUiServiceProxy;
        NexusAssetManager = nexusAssetManager;
        NexusComponentManager = nexusComponentManager;
        NexusLookupService = nexusLookupService;
        CurrentTenant = currentTenant;
        BlobRawPathCalculator = blobRawPathCalculator;
        ConfigurationProvider = configurationProvider;
    }

    public Task BulkDeleteObjectsAsync(BulkDeleteObjectRequest request)
    {
        throw new NotImplementedException();
    }

    public async virtual Task<OssContainer> CreateAsync(string name)
    {
        // 创建容器的逻辑就是创建一个包含一个空白assets的component

        var blobPath = BlobRawPathCalculator.CalculateGroup(name, "readme");

        var nexusConfiguration = GetNexusConfiguration();

        var uploadArgs = new NexusRawBlobUploadArgs(
            nexusConfiguration.Repository,
            blobPath,
            new Asset("readme", Encoding.UTF8.GetBytes("A placeholder for an empty container.")));

        await NexusComponentManager.UploadAsync(uploadArgs);

        return new OssContainer(
            name,
            DateTime.Now,
            0L,
            DateTime.Now);
    }

    public async virtual Task<OssObject> CreateObjectAsync(CreateOssObjectRequest request)
    {
        var nexusConfiguration = GetNexusConfiguration();
        var blobPath = GetBasePath(request.Bucket, request.Path, request.Object);
        if (!request.Overwrite)
        {
            var searchBlobName = GetObjectName(request.Bucket, request.Path, request.Object);
            var nexusSearchArgs = new NexusSearchArgs(
               nexusConfiguration.Repository,
               blobPath,
               searchBlobName);

            var nexusAssetListResult = await NexusLookupService.ListAssetAsync(nexusSearchArgs);
            if (nexusAssetListResult.Items.Any())
            {
                throw new BusinessException(code: OssManagementErrorCodes.ObjectAlreadyExists);
            }
        }

        var blobName = GetObjectName(request.Bucket, request.Path, request.Object, true);
        var blobBytes = await request.Content.GetAllBytesAsync();
        var uploadArgs = new NexusRawBlobUploadArgs(
           nexusConfiguration.Repository,
           blobPath,
           new Asset(blobName, blobBytes));

        await NexusComponentManager.UploadAsync(uploadArgs);

        var getOssObjectRequest = new GetOssObjectRequest(
            request.Bucket, request.Object, request.Path);

        return await GetObjectAsync(getOssObjectRequest);
    }

    public async virtual Task DeleteAsync(string name)
    {
        var nexusConfiguration = GetNexusConfiguration();
        var blobPath = BlobRawPathCalculator.CalculateGroup(name, "/");

        var nexusSearchArgs = new NexusSearchArgs(
              nexusConfiguration.Repository,
              blobPath,
              "/");

        var nexusComponentListResult = await NexusLookupService.ListComponentAsync(nexusSearchArgs);
        var nexusComponent = nexusComponentListResult.Items.FirstOrDefault();
        if (nexusComponent != null)
        {
            await NexusComponentManager.DeleteAsync(nexusComponent.Id);
        }
    }

    public async virtual Task DeleteObjectAsync(GetOssObjectRequest request)
    {
        var nexusConfiguration = GetNexusConfiguration();
        var blobPath = GetBasePath(request.Bucket, request.Path, request.Object);
        var blobName = GetObjectName(request.Bucket, request.Path, request.Object);

        var nexusSearchArgs = new NexusSearchArgs(
              nexusConfiguration.Repository,
              blobPath,
              blobName);

        var nexusAssetListResult = await NexusLookupService.ListAssetAsync(nexusSearchArgs);
        var nexusAsset = nexusAssetListResult.Items.FirstOrDefault();
        if (nexusAsset != null)
        {
            await NexusAssetManager.DeleteAsync(nexusAsset.Id);
        }
    }

    public virtual Task<bool> ExistsAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task ExpireAsync(ExprieOssObjectRequest request)
    {
        throw new NotImplementedException();
    }

    public async virtual Task<OssContainer> GetAsync(string name)
    {
        var nexusConfiguration = GetNexusConfiguration();
        var blobPath = BlobRawPathCalculator.CalculateGroup(name, "/");

        var nexusSearchArgs = new NexusSearchArgs(
              nexusConfiguration.Repository,
              blobPath,
              "/");

        var nexusComponentListResult = await NexusLookupService.ListComponentAsync(nexusSearchArgs);
        var nexusComponent = nexusComponentListResult.Items.FirstOrDefault();
        if (nexusComponent == null)
        {
            throw new BusinessException(code: OssManagementErrorCodes.ContainerNotFound);
        }

        var lastModified = nexusComponent.Assets
            .OrderBy(asset => asset.LastModified)
            .Select(asset => asset.LastModified)
            .FirstOrDefault();

        return new OssContainer(
            nexusComponent.Name,
            lastModified ?? new DateTime(),
            0L,
            lastModified);
    }

    public async virtual Task<GetOssContainersResponse> GetListAsync(GetOssContainersRequest request)
    {
        var nexusConfiguration = GetNexusConfiguration();
        var blobPath = request.Prefix.RemovePreFix(".").RemovePreFix("/");
        var readComponent = CoreUIBrowse.Read(nexusConfiguration.Repository, blobPath);

        var coreUIResponse = await CoreUiServiceProxy.SearchAsync<CoreUIBrowseReadComponent, CoreUIBrowseComponentResult>(readComponent);
        var ifFolderComponents = coreUIResponse.Result.Data.Where(component => component.Type == "folder").ToArray();

        return new GetOssContainersResponse(
            request.Prefix,
            request.Marker,
            "",
            ifFolderComponents.Length,
            ifFolderComponents.Select(component =>
                new OssContainer(
                    component.Text,
                    new DateTime(),
                    0L,
                    null,
                    new Dictionary<string, string>()))
            .ToList());
    }

    public async virtual Task<OssObject> GetObjectAsync(GetOssObjectRequest request)
    {
        var nexusConfiguration = GetNexusConfiguration();
        var blobPath = GetBasePath(request.Bucket, request.Path, request.Object);
        var blobFullName = GetObjectName(request.Bucket, request.Path, request.Object);
        var blobName = GetObjectName(request.Bucket, request.Path, request.Object, true);

        var nexusSearchArgs = new NexusSearchArgs(
              nexusConfiguration.Repository,
              blobPath,
              blobFullName);

        var nexusAssetListResult = await NexusLookupService.ListAssetAsync(nexusSearchArgs);
        var nexusAssetItem = nexusAssetListResult.Items.FirstOrDefault();
        if (nexusAssetItem == null)
        {
            throw new BusinessException(code: OssManagementErrorCodes.ObjectNotFound);
        }

        var (Repository, ComponentId) = DecodeBase64Id(nexusAssetItem.Id);

        var readAsset = CoreUIAsset.Read(ComponentId, Repository);

        var coreUIResponse = await CoreUiServiceProxy.SearchAsync<CoreUIAssetRead, CoreUIAssetResult>(readAsset);
        var checksum = coreUIResponse.Result.Data.Attributes.GetOrDefault("checksum");
        var metadata = new Dictionary<string, string>();
        if (checksum != null)
        {
            foreach (var data in checksum)
            {
                metadata.Add(data.Key, data.Value.ToString());
            }
        }

        return new OssObject(
            blobName,
            blobPath,
            checksum?.GetOrDefault("md5")?.ToString(),
            coreUIResponse.Result.Data.BlobCreated,
            coreUIResponse.Result.Data.Size,
            coreUIResponse.Result.Data.BlobUpdated,
            metadata
            );
    }

    public async virtual Task<GetOssObjectsResponse> GetObjectsAsync(GetOssObjectsRequest request)
    {
        var nexusConfiguration = GetNexusConfiguration();
        var blobPath = GetBasePath(request.BucketName, request.Prefix, "");
        var readComponent = CoreUIBrowse.Read(nexusConfiguration.Repository, blobPath.RemovePreFix("/"));

        var coreUIResponse = await CoreUiServiceProxy.SearchAsync<CoreUIBrowseReadComponent, CoreUIBrowseComponentResult>(readComponent);
        var filterComponents = coreUIResponse.Result.Data
            .WhereIf(string.Equals(request.Delimiter, "/"), component => component.Type == "folder")
            .OrderBy(component => component.Text)
            .AsQueryable()
            .PageBy(request.Current, request.MaxKeys ?? 10)
            .ToArray();

        var response = new GetOssObjectsResponse(
                request.BucketName,
                request.Prefix,
                request.Marker,
                "",
                "/", // 文件系统目录分隔符
                coreUIResponse.Result.Data.Count,
                filterComponents.Select(component => new OssObject(
                    component.Text,
                    request.Prefix,
                    "",
                    null,
                    0L,
                    null,
                    new Dictionary<string, string>(),
                    component.Type == "folder")
                {
                    FullName = component.Id
                })
                .ToList());

        return response;
    }

    protected virtual NexusBlobProviderConfiguration GetNexusConfiguration()
    {
        var configuration = ConfigurationProvider.Get<AbpOssManagementContainer>();
        var nexusConfiguration = configuration.GetNexusConfiguration();
        return nexusConfiguration;
    }

    protected virtual string GetBasePath(string bucket, string path, string @object)
    {
        var objectPath = bucket.EnsureEndsWith('/') + (!path.IsNullOrWhiteSpace() ? path.RemovePreFix("/") : "");
        objectPath = BlobRawPathCalculator.CalculateGroup(objectPath, @object);
        return objectPath;
    }

    protected virtual string GetObjectName(string bucket, string path, string @object, bool replaceObjectPath = false)
    {
        var objectPath = bucket.EnsureEndsWith('/') + (!path.IsNullOrWhiteSpace() ? path.RemovePreFix("/") : "");
        //objectPath = BlobRawPathCalculator.CalculateGroup(objectPath, @object);
        var objectName = BlobRawPathCalculator.CalculateName(objectPath, @object, replaceObjectPath);
        return objectName;
    }

    protected virtual (string Repository, string ComponentId) DecodeBase64Id(string base64id)
    {
        base64id = base64id.Replace("-", "+").Replace("_", "/");
        var base64 = Encoding.ASCII.GetBytes(base64id);
        var padding = base64.Length * 3 % 4;//(base64.Length*6 % 8)/2
        if (padding != 0)
        {
            base64id = base64id.PadRight(base64id.Length + padding, '=');
        }

        var buffer = Convert.FromBase64String(base64id);
        var decoded = Encoding.UTF8.GetString(buffer);
        var parts = decoded.Split(":");
        if (parts.Length != 2)
        {
            throw new AbpException("Unable to parse component id " + decoded);
        }
        return (parts[0], parts[1]);
    }
}
