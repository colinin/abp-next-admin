using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.ElsaNext.BlobStoring.Activities;

public abstract class BlobActivity : CodeActivity
{
    [Input(Description = "Path of the blob.")]
    public Input<string> Path { get; set; } = default!;

    protected virtual IBlobContainer<ElsaBlobContainer> GetBlobContainer(ActivityExecutionContext context)
    {
        return context.GetRequiredService<IBlobContainer<ElsaBlobContainer>>();
    }

    protected async virtual ValueTask OnErrorCompletedAsync(ActivityCompletedContext context)
    {
        await context.TargetContext.CompleteActivityAsync();
    }
}

public abstract class BlobActivity<TResult> : CodeActivity<TResult>
{
    [Input(Description = "Path of the blob.")]
    public Input<string> Path { get; set; } = default!;

    protected virtual IBlobContainer<ElsaBlobContainer> GetBlobContainer(ActivityExecutionContext context)
    {
        return context.GetRequiredService<IBlobContainer<ElsaBlobContainer>>();
    }

    protected async virtual ValueTask OnErrorCompletedAsync(ActivityCompletedContext context)
    {
        await context.TargetContext.CompleteActivityAsync();
    }
}
