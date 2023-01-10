using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

[Action(Category = "Blob",
        Description = "Check if a blob exists.",
        Outcomes = new[] { OutcomeNames.True, OutcomeNames.False })]
public class BlobExists : BlobActivity
{
    public BlobExists(IBlobContainer<ElsaBlobContainer> blobContainer)
        : base(blobContainer)
    {
    }

    protected async override ValueTask<IActivityExecutionResult> OnActivitExecuteAsync(ActivityExecutionContext context)
    {
        var exists = await BlobContainer.ExistsAsync(Path, context.CancellationToken);
        if (exists)
        {
            return Outcome(OutcomeNames.True);
        }
        return Outcome(OutcomeNames.False);
    }
}
