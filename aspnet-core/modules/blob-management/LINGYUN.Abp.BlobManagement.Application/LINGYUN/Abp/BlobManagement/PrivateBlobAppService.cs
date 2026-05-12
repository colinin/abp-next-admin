using LINGYUN.Abp.BlobManagement.Features;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Features;

namespace LINGYUN.Abp.BlobManagement;

[Authorize]
[RequiresFeature(BlobManagementFeatureNames.Blob.Enable)]
public class PrivateBlobAppService : BlobWithoutContainerAppService, IPrivateBlobAppService
{
    protected override string ContainerName => "users";
    public PrivateBlobAppService(
        IBlobContainerRepository blobContainerRepository, 
        IBlobRepository blobRepository, 
        BlobManager blobManager) 
        : base(blobContainerRepository, blobRepository, blobManager)
    {
    }
}
