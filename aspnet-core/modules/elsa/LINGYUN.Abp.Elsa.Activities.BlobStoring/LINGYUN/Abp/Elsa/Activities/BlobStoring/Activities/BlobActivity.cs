using Elsa.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

public abstract class BlobActivity : Activity
{
    protected ICurrentTenant CurrentTenant;
    protected IBlobContainer<ElsaBlobContainer> BlobContainer;

    protected BlobActivity(IBlobContainer<ElsaBlobContainer> blobContainer)
    {
        BlobContainer = blobContainer;
    }
}
