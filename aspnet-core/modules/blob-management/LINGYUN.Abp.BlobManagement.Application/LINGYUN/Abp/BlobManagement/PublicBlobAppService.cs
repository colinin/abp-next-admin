using LINGYUN.Abp.BlobManagement.Features;
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
    }

    protected override Task CheckGetPolicyAsync(string containerName, Blob blob)
    {
        // 公共文件访问不校验权限
        return Task.CompletedTask;
    }

    protected override Task CheckDeletePolicyAsync(string containerName, Blob blob)
    {
        return base.CheckDeletePolicyAsync(blob);
    }
}
