using System.IO;
using System.Text.Json.Serialization;
using Volo.Abp.Content;

namespace LINGYUN.Abp.ElsaNext.BlobStoring.Models;

public class BlobContent
{
    public Stream Stream { get; set; } = default!;

    public string? Filename { get; set; }

    public string? ContentType { get; set; }

    [JsonConstructor]
    public BlobContent()
    {

    }

    public BlobContent(Stream stream, string? filename = default, string? contentType = default)
    {
        Stream = stream;
        Filename = filename;
        ContentType = contentType;
    }

    public byte[] GetBytes()
    {
        using var memoryStream = new MemoryStream();
        if (Stream.CanSeek)
        {
            Stream.Seek(0, SeekOrigin.Begin);
        }
        Stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    public RemoteStreamContent GetRemoteStreamContent()
    {
        return new RemoteStreamContent(Stream, Filename, ContentType);
    }
}
