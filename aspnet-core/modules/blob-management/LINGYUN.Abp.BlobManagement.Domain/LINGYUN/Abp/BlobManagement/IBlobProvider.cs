using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobManagement;

public interface IBlobProvider
{
    string Name { get; }

    Task CreateContainerAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task DeleteContainerAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<Stream?> DownloadBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default);

    Task CreateFolderAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default);

    Task DeleteFolderAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default);

    Task UploadBlobAsync(
        string containerName,
        string blobName,
        Stream stream,
        CancellationToken cancellationToken = default);

    Task DeleteBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default);
}
