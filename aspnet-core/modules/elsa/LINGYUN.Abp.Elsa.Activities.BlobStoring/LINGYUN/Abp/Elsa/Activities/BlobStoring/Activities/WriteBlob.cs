using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

[Action(Category = "Blob",
        Description = "Save a blob.",
        Outcomes = new[] { OutcomeNames.Done })]
public class WriteBlob : Activity
{
    private readonly IBlobContainer<AbpElsaBlobContainer> _container;

    [ActivityInput(Hint = "Path of the blob.")]
    public string? Path { get; set; }

    [ActivityInput(Hint = "Blob exists whether overwrite")]
    public bool Overwrite { get; set; }

    [ActivityInput(Hint = "The bytes to write.", SupportedSyntaxes = new[] { SyntaxNames.JavaScript }, DefaultSyntax = SyntaxNames.JavaScript)]
    public byte[]? Bytes { get; set; }

    public WriteBlob(IBlobContainer<AbpElsaBlobContainer> container)
    {
        _container = container;
    }

    protected async override ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        await _container.SaveAsync(Path, Bytes, Overwrite, context.CancellationToken);

        return Done();
    }
}
