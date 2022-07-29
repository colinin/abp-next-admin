using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

[Action(Category = "Blob",
        Description = "Reads a blob.",
        Outcomes = new[] { OutcomeNames.Done })]
public class ReadBlob : BlobActivity
{
    [ActivityInput(Hint = "Path of the blob.")]
    public string? Path { get; set; }

    [ActivityOutput]
    public byte[]? Output { get; set; }

    public ReadBlob(IBlobContainer<ElsaBlobContainer> blobContainer)
        : base(blobContainer)
    {
    }

    protected async override ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        Output = await BlobContainer.GetAllBytesAsync(Path, context.CancellationToken);

        return Done();
    }
}
