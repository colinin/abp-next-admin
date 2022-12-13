using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Platform.Packages;

public interface IPackageBlobManager
{
    Task RemoveBlobAsync(
        Package package,
        PackageBlob packageBlob,
        CancellationToken cancellationToken = default);

    Task SaveBlobAsync(
        Package package,
        PackageBlob packageBlob,
        Stream stream,
        bool overrideExisting = true,
        CancellationToken cancellationToken = default);

    Task<Stream> DownloadBlobAsync(
        Package package,
        PackageBlob packageBlob, 
        CancellationToken cancellationToken = default);
}
