using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.Nexus;
public class NexusBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
{
    public virtual string NormalizeContainerName(string containerName)
    {
        return Normalize(containerName);
    }

    public virtual string NormalizeBlobName(string blobName)
    {
        return Normalize(blobName);
    }

    protected virtual string Normalize(string fileName)
    {
        return fileName.Replace("\\", "/").Replace("//", "/");
    }
}
