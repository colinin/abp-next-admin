using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.OssManagement;

public class OssManagementBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
{
    public virtual string NormalizeBlobName(string blobName)
    {
        return NormalizeName(blobName);
    }

    public virtual string NormalizeContainerName(string containerName)
    {
        return NormalizeName(containerName);
    }

    protected virtual string NormalizeName(string name)
    {
        // 取消路径修饰符
        name = name.Replace("./", "").Replace("../", "");
        // 取消反斜杠开头
        if (name.StartsWith("/"))
        {
            name = name.Substring(1);
        }

        return name;
    }
}
