using System.Threading;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.FileManagement.FileSystem
{
    public class FileSystemOssProviderArgs : BlobProviderArgs
    {
        public FileSystemOssProviderArgs(
            string containerName, 
            BlobContainerConfiguration configuration, 
            string blobName, 
            CancellationToken cancellationToken = default) 
            : base(containerName, configuration, blobName, cancellationToken)
        {

        }
    }
}
