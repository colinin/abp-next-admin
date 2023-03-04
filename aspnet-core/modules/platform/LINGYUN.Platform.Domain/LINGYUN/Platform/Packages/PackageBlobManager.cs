using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Platform.Packages;

[Dependency(TryRegister = true)]
public class PackageBlobManager : DomainService, IPackageBlobManager, ITransientDependency
{
    protected IPackageBlobNormalizer BlobNormalizer { get; }
    protected IBlobContainer<PackageContainer> PackageContainer { get; }

    public PackageBlobManager(
        IPackageBlobNormalizer blobNormalizer, 
        IBlobContainer<PackageContainer> packageContainer)
    {
        BlobNormalizer = blobNormalizer;
        PackageContainer = packageContainer;
    }

    public async virtual Task RemoveBlobAsync(
        Package package,
        PackageBlob packageBlob,
        CancellationToken cancellationToken = default)
    {
        var blobName = BlobNormalizer.Normalize(package, packageBlob);

        await RemoveBlobAsync(blobName);
    }

    public async virtual Task<Stream> DownloadBlobAsync(
        Package package,
        PackageBlob packageBlob, 
        CancellationToken cancellationToken = default)
    {
        packageBlob.Download();

        return await DownloadFromBlobAsync(package, packageBlob);
    }

    public async virtual Task SaveBlobAsync(
        Package package,
        PackageBlob packageBlob, 
        Stream stream,
        bool overrideExisting = true,
        CancellationToken cancellationToken = default)
    {
        var blobName = BlobNormalizer.Normalize(package, packageBlob);

        await SaveToBlobAsync(blobName, stream, overrideExisting, cancellationToken);

        stream.Seek(0, SeekOrigin.Begin);
        packageBlob.SHA256 = ComputeHash(stream);
        packageBlob.SetUrl($"api/platform/packages/{packageBlob.PackageId}/blob/{HttpUtility.HtmlEncode(packageBlob.Name)}");
    }

    protected async virtual Task<Stream> DownloadFromBlobAsync(
        Package package,
        PackageBlob packageBlob,
        CancellationToken cancellationToken = default)
    {
        var blobName = BlobNormalizer.Normalize(package, packageBlob);

        return await PackageContainer.GetAsync(blobName);
    }

    protected async virtual Task SaveToBlobAsync(
        string blobName,
        Stream stream,
        bool overrideExisting = true,
        CancellationToken cancellationToken = default)
    {
        await PackageContainer.SaveAsync(blobName, stream, overrideExisting, cancellationToken);
    }

    protected async virtual Task RemoveBlobAsync(
        string blobName,
        CancellationToken cancellationToken = default)
    {
        await PackageContainer.DeleteAsync(blobName, cancellationToken);
    }

    protected virtual string ComputeHash(Stream stream)
    {
        using var sha256 = SHA256.Create();
        var checkHash = sha256.ComputeHash(stream);
        return BitConverter.ToString(checkHash).Replace("-", string.Empty);
    }
}
