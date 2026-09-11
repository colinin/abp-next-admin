using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.BlobStoring.Activities;

[Activity("Elsa", "BlobStoring", "Open a blob.", Kind = ActivityKind.Task)]
public class ReadBlob : BlobActivity<Stream>
{
    [Port] public IActivity? Error { get; set; }

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;
        var path = Path.Get(context);
        var blobContainer = GetBlobContainer(context);

        var logger = context.GetRequiredService<ILogger<ReadBlob>>();
        try
        {
            var data = await blobContainer.GetAsync(path, cancellationToken);

            Result.Set(context, data);
            await context.CompleteActivityAsync();
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Error while open a blob.");
            context.AddExecutionLogEntry("Error", e.Message, payload: new
            {
                e.StackTrace
            });
            await context.ScheduleActivityAsync(Error, OnErrorCompletedAsync);
        }
    }
}
