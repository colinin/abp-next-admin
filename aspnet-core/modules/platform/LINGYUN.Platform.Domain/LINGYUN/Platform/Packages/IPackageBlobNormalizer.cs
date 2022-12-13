namespace LINGYUN.Platform.Packages;

public interface IPackageBlobNormalizer
{
    string Normalize(Package package, PackageBlob blob);
}
