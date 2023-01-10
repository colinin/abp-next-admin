using Elsa.Attributes;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

public abstract class BlobActivity : AbpActivity
{
    [ActivityInput(Hint = "Path of the blob.")]
    public string Path { get; set; }

    protected IBlobContainer<ElsaBlobContainer> BlobContainer;

    protected BlobActivity(IBlobContainer<ElsaBlobContainer> blobContainer)
    {
        BlobContainer = blobContainer;
    }
}
