using System.IO;
using System.Web;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Platform.Packages;

public class FileSystemPackageBlobNormalizer : IPackageBlobNormalizer, ISingletonDependency
{
    public FileSystemPackageBlobNormalizer()
    {

    }

    public string Normalize(Package package, PackageBlob blob)
    {
        var pk = package.Name;
        var pv = package.Version;

        return Path.Combine(pk, "v" + pv, "blobs", HttpUtility.HtmlDecode(blob.Name));
    }
}
