using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.BlobManagement;

public class BlobManagementBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
{
    public virtual string NormalizeBlobName(string blobName)
    {
        return blobName;
    }

    public virtual string NormalizeContainerName(string containerName)
    {
        return containerName;
    }
}
