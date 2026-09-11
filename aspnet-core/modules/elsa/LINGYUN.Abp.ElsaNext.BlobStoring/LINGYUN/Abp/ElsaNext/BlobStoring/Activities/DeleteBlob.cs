using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.BlobStoring.Activities;

[Activity("Elsa", "BlobStoring", "Delete a blob.", Kind = ActivityKind.Task)]
public class DeleteBlob : BlobActivity<bool>
{
    protected async override ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;
        var path = Path.Get(context);
        var blobContainer = GetBlobContainer(context);
        var result = await blobContainer.DeleteAsync(path, cancellationToken);

        Result.Set(context, result);
    }
}
