using LINGYUN.Abp.BlobManagement.Features;
using LINGYUN.Abp.BlobManagement.Permissions;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.BlobManagement;

[RequiresFeature(BlobManagementFeatureNames.Blob.Enable)]
public class PublicBlobAppService : BlobWithoutContainerAppService, IPublicBlobAppService
{
    protected override string ContainerName => "public";

    public PublicBlobAppService(
        IBlobContainerRepository blobContainerRepository, 
        IBlobRepository blobRepository, 
        BlobManager blobManager) 
        : base(blobContainerRepository, blobRepository, blobManager)
    {
        DeletePolicyName = BlobManagementPermissionNames.Blob.Delete;
    }

    protected async override Task CheckGetPolicyAsync(string containerName, Blob blob)
    {
        if (!CurrentUser.IsAuthenticated)
        {
            await FeatureChecker.CheckEnabledAsync(BlobManagementFeatureNames.PublicAccess);
        }
    }

    protected async override Task CheckDeletePolicyAsync(string containerName, Blob blob)
    {
        await CheckPolicyAsync(DeletePolicyName);
    }
}
