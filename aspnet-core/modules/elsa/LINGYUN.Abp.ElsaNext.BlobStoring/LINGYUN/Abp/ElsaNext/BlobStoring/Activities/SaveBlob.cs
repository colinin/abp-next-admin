using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.ElsaNext.BlobStoring.Activities;

[Activity("Elsa", "BlobStoring", "Save a blob.", Kind = ActivityKind.Task)]
public class SaveBlob : BlobActivity
{
    [Input(Description = "Blob exists whether overwrite.")]
    public Input<bool> Overwrite { get; set; } = null!;

    [Input(Description = "The file data to save. This can be a stream, binary data, a string, a form file or a collection of files.")]
    public Input<object> Data { get; set; } = null!;

    [Port] public IActivity? Error { get; set; }

    protected async override ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;
        var data = await ResolveAsStreamAsync(Data.Get(context), cancellationToken);
        var overwrite = Overwrite.GetOrDefault(context);
        var path = Path.Get(context);
        var blobContainer = GetBlobContainer(context);

        var logger = context.GetRequiredService<ILogger<SaveBlob>>();
        try
        {
            await blobContainer.SaveAsync(path, data, overwrite, cancellationToken);
            await context.CompleteActivityAsync();
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Error while save a blob.");
            context.AddExecutionLogEntry("Error", e.Message, payload: new
            {
                e.StackTrace
            });
            await context.ScheduleActivityAsync(Error, OnErrorCompletedAsync);
        }
    }

    private async Task<Stream> ResolveAsStreamAsync(object data, CancellationToken cancellationToken)
    {
        if (data is Stream stream)
            return stream;

        if (data is byte[] bytes)
            return new MemoryStream(bytes);

        if (data is IFormFile formFile)
            return formFile.OpenReadStream();

        if (data is string stringData)
            return new MemoryStream(Encoding.UTF8.GetBytes(stringData));

        if (data is IEnumerable enumerable)
        {
            var files = enumerable.Cast<object>().ToList();
            return files.Count == 1 ? await ResolveAsStreamAsync(files[0], cancellationToken) : await CreateZipArchiveAsync(files, cancellationToken);
        }

        throw new NotSupportedException($"The provided data type is not supported: {data.GetType().Name}");
    }

    private async Task<Stream> CreateZipArchiveAsync(IEnumerable files, CancellationToken cancellationToken = default)
    {
        var currentFileIndex = 0;
        var zipStream = new MemoryStream();
        var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true);

        foreach (var file in files)
        {
            var entryName = $"file-{currentFileIndex}.bin";
            var entry = zipArchive.CreateEntry(entryName);
            var fileStream = await ResolveAsStreamAsync(file, cancellationToken);
            await using var entryStream = entry.Open();
            await fileStream.CopyToAsync(entryStream, cancellationToken);
            await entryStream.FlushAsync(cancellationToken);
            entryStream.Close();
            currentFileIndex++;
        }

        zipStream.Seek(0, SeekOrigin.Begin);
        return zipStream;
    }
}
