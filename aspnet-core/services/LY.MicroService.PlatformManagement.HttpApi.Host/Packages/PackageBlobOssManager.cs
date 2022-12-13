using LINGYUN.Abp.OssManagement;
using LINGYUN.Platform.Packages;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LY.MicroService.PlatformManagement.Packages;

/// <summary>
/// 使用Oss模块管理包二进制文件
/// </summary>
[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(IPackageBlobManager),
    typeof(PackageBlobManager),
    typeof(PackageBlobOssManager))]
public class PackageBlobOssManager : PackageBlobManager, ITransientDependency
{
    protected IOssObjectAppService ObjectAppService { get; }

    public PackageBlobOssManager(
        IPackageBlobNormalizer blobNormalizer, 
        IBlobContainer<PackageContainer> packageContainer,
        IOssObjectAppService objectAppService) 
        : base(blobNormalizer, packageContainer)
    {
        ObjectAppService = objectAppService;
    }

    public async override Task RemoveBlobAsync(
        Package package,
        PackageBlob packageBlob,
        CancellationToken cancellationToken = default)
    {
        var blobName = BlobNormalizer.Normalize(package, packageBlob);
        var bucket = BlobContainerNameAttribute.GetContainerName<PackageContainer>();
        var path = blobName.Replace(packageBlob.Name, "");
        path.RemovePostFix(Path.PathSeparator.ToString());

        var input = new GetOssObjectInput
        {
            Bucket = bucket,
            Path = path,
            Object = packageBlob.Name
        };
        await ObjectAppService.DeleteAsync(input);
    }

    protected async override Task<Stream> DownloadFromBlobAsync(
        Package package,
        PackageBlob packageBlob, 
        CancellationToken cancellationToken = default)
    {
        var blobName = BlobNormalizer.Normalize(package, packageBlob);
        var bucket = BlobContainerNameAttribute.GetContainerName<PackageContainer>();
        var path = blobName.Replace(packageBlob.Name, "");
        path.RemovePostFix(Path.PathSeparator.ToString());

        var input = new GetOssObjectInput
        {
            Bucket = bucket,
            Path = path,
            Object = packageBlob.Name
        };
        var ossContent = await ObjectAppService.GetContentAsync(input);
        return ossContent.GetStream();
    }

    public async override Task SaveBlobAsync(
        Package package,
        PackageBlob packageBlob, 
        Stream stream, 
        bool overrideExisting = true, 
        CancellationToken cancellationToken = default)
    {
        var blobName = BlobNormalizer.Normalize(package, packageBlob);
        var bucket = BlobContainerNameAttribute.GetContainerName<PackageContainer>();
        var path = blobName.Replace(packageBlob.Name, "");

        var input = new CreateOssObjectInput
        {
            Bucket = bucket,
            Path = path,
            FileName= packageBlob.Name,
            Overwrite= overrideExisting,
            File = new RemoteStreamContent(stream, packageBlob.Name, packageBlob.ContentType)
        };

        var ossObject = await ObjectAppService.CreateAsync(input);

        if (!ossObject.MD5.IsNullOrWhiteSpace())
        {
            packageBlob.SetProperty("md5", ossObject.MD5);
        }

        stream.Seek(0, SeekOrigin.Begin);
        packageBlob.SHA256 = ComputeHash(stream);
        packageBlob.SetUrl($"api/platform/packages/{packageBlob.PackageId}/blob/{packageBlob.Name}");
    }
}
