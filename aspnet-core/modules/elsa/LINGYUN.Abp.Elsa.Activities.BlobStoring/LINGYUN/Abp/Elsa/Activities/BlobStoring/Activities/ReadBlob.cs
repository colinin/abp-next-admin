using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

[Action(Category = "Blob",
        Description = "Reads a blob.",
        Outcomes = new[] { OutcomeNames.Done })]
public class ReadBlob : Activity
{
    private readonly IBlobContainer<AbpElsaBlobContainer> _container;

    [ActivityInput(Hint = "Path of the blob.")]
    public string? Path { get; set; }

    [ActivityOutput]
    public byte[]? Output { get; set; }

    public ReadBlob(IBlobContainer<AbpElsaBlobContainer> container)
    {
        _container = container;
    }

    protected async override ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        Output = await _container.GetAllBytesAsync(Path, context.CancellationToken);

        return Done();
    }
}
