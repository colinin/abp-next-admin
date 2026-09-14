using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using LINGYUN.Abp.ElsaNext.BlobStoring.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace LINGYUN.Abp.ElsaNext.BlobStoring.Activities;

[Activity("Elsa", "BlobStoring", "Open a blob.", Kind = ActivityKind.Task)]
public class ReadBlob : BlobActivity<BlobContent>
{
    /// <summary>
    /// The blob content stream.
    /// </summary>
    [Output(Description = "The blob content stream.", IsSerializable = false)]
    public Output<Stream> ContentStream { get; set; } = null!;

    [Port] public IActivity? Error { get; set; }

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;
        var path = Path.Get(context);
        var blobContainer = GetBlobContainer(context);

        var logger = context.GetRequiredService<ILogger<ReadBlob>>();
        try
        {
            var stream = await blobContainer.GetAsync(path, cancellationToken);
            var fileName = System.IO.Path.GetFileName(path);
            var contentType = "application/octet-stream";
            if (stream is IRemoteStreamContent remoteStreamContent)
            {
                fileName = remoteStreamContent.FileName ?? fileName;
                contentType = remoteStreamContent.ContentType;
            }
            else if(stream is FileStream fileStream)
            {
                fileName = fileStream.Name ?? fileName;
            }
            context.Set(ContentStream, stream);
            context.Set(Result, new BlobContent(stream, fileName, contentType));

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
