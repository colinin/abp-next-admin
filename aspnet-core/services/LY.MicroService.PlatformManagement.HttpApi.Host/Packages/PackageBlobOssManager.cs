using LINGYUN.Abp.OssManagement;
using LINGYUN.Platform.Packages;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
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
    protected IOssContainerFactory OssContainerFactory { get; }

    public PackageBlobOssManager(
        IPackageBlobNormalizer blobNormalizer, 
        IBlobContainer<PackageContainer> packageContainer,
        IOssContainerFactory ossContainerFactory) 
        : base(blobNormalizer, packageContainer)
    {
        OssContainerFactory = ossContainerFactory;
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

        var oss = OssContainerFactory.Create();

        await oss.DeleteObjectAsync(bucket, packageBlob.Name, path);
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

        var oss = OssContainerFactory.Create();

        var ossObject = await oss.GetObjectAsync(bucket, packageBlob.Name, path);

        return ossObject.Content;
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

        var request = new CreateOssObjectRequest(
                    bucket,
                    packageBlob.Name,
                    stream,
                    path)
        {
            Overwrite = overrideExisting
        };

        var oss = OssContainerFactory.Create();
        var ossObject = await oss.CreateObjectAsync(request);

        if (!ossObject.MD5.IsNullOrWhiteSpace())
        {
            packageBlob.SetProperty("md5", ossObject.MD5);
        }

        stream.Seek(0, SeekOrigin.Begin);
        packageBlob.SHA256 = ComputeHash(stream);
        packageBlob.SetUrl($"api/platform/packages/{packageBlob.PackageId}/blob/{packageBlob.Name}");
    }
}
