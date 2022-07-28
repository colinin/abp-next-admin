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
        Description = "Deletes blob at specified location",
        Outcomes = new[] { OutcomeNames.Done })]
public class DeleteBlob : Activity
{
    private readonly IBlobContainer<AbpElsaBlobContainer> _container;

    [ActivityInput(Hint = "Path of the blob to be deleted.")]
    public string? Path { get; set; }

    public DeleteBlob(IBlobContainer<AbpElsaBlobContainer> container)
    {
        _container = container;
    }

    protected async override ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        await _container.DeleteAsync(Path, context.CancellationToken);

        return Done();
    }
}
