using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.BlobManagement;

public class BlobManagementBlobProviderConfiguration
{
    private readonly BlobContainerConfiguration _containerConfiguration;

    public BlobManagementBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
    {
        _containerConfiguration = containerConfiguration;
    }
}
