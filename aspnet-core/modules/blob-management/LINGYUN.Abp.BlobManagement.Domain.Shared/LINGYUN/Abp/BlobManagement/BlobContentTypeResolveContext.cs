using System;
using System.IO;

namespace LINGYUN.Abp.BlobManagement;

public class BlobContentTypeResolveContext
{
    public IServiceProvider ServiceProvider { get; }
    public string BlobName { get; }
    public Stream Stream { get; }
    public bool Handled { get; private set; }
    public string? ContentType { get; private set; }
    public BlobContentTypeResolveContext(
        IServiceProvider serviceProvider,
        string blobName, 
        Stream stream)
    {
        ServiceProvider = serviceProvider;
        BlobName = blobName;
        Stream = stream;
    }

    public void Success(string contentType)
    {
        ContentType = contentType;
        Handled = true;
    }

    public bool HasResolvedContentType()
    {
        return Handled && !ContentType.IsNullOrWhiteSpace();
    }
}
