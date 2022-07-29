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
    [ActivityInput(Hint = "Path of the blob to be deleted.")]
    public string? Path { get; set; }

    public DeleteBlob(IBlobContainer<ElsaBlobContainer> blobContainer)
        : base(blobContainer)
    {
    }

    protected async override ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        await BlobContainer.DeleteAsync(Path, context.CancellationToken);

        return Done();
    }
}
