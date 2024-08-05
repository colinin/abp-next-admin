using JetBrains.Annotations;
using System;
using System.IO;
using Volo.Abp;

namespace LINGYUN.Abp.OssManagement;

public class CreateOssObjectRequest
{
    public string Bucket { get; }
    public string Path { get; }
    public string Object { get; }
    public bool Overwrite { get; set; }
    public Stream Content { get; private set; }
    public TimeSpan? ExpirationTime { get; }
    public CreateOssObjectRequest(
        [NotNull] string bucket,
        [NotNull] string @object,
        [CanBeNull] Stream content,
        [CanBeNull] string path = null,
        [CanBeNull] TimeSpan? expirationTime = null)
    {
        Bucket = Check.NotNullOrWhiteSpace(bucket, nameof(bucket));
        Object = Check.NotNullOrWhiteSpace(@object, nameof(@object));

        Path = path;
        Content = content;
        ExpirationTime = expirationTime;
    }

    public void SetContent(Stream content)
    {
        Content = content;
    }
}
