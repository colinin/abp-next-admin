using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

[Action(Category = "Blob",
        Description = "Deletes blob at specified location",
        Outcomes = new[] { OutcomeNames.Done })]
public class DeleteBlob : BlobActivity
{
    public DeleteBlob(IBlobContainer<ElsaBlobContainer> blobContainer)
        : base(blobContainer)
    {
    }

    protected async override ValueTask<IActivityExecutionResult> OnActivityExecuteAsync(ActivityExecutionContext context)
    {
        await BlobContainer.DeleteAsync(Path, context.CancellationToken);

        return Done();
    }
}
