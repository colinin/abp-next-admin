using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Nexus;
public interface IBlobRawPathCalculator
{
    string CalculateGroup(string containerName, string blobName);

    string CalculateName(string containerName, string blobName, bool replacePath = false);
}
