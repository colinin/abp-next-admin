using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LINGYUN.Abp.Sonatype.Nexus.Components;
public class NexusRawBlobUploadArgs : NexusComponentUploadArgs
{
    public Asset Asset1 { get; }
    public Asset Asset2 { get; }
    public Asset Asset3 { get; }

    public NexusRawBlobUploadArgs(
        string repository,
        string directory,
        Asset asset1,
        Asset asset2 = null,
        Asset asset3 = null)
        : base(repository, directory)
    {
        Asset1 = asset1;
        Asset2 = asset2;
        Asset3 = asset3;
    }

    public override HttpContent BuildContent()
    {
        var boundary = "--BOUNDARY--" + DateTimeOffset.Now.Ticks.ToString("x");
        var formDataContent = new MultipartFormDataContent(boundary);
        formDataContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"multipart/form-data; boundary={boundary}");

        var rawDirectory = new StringContent(Directory);
        rawDirectory.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=raw.directory");

        if (Asset1 != null)
        {
            var rawAsset1 = new ByteArrayContent(Asset1.FileBytes);
            rawAsset1.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=raw.asset1");
            rawAsset1.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            rawAsset1.Headers.ContentLength = Asset1.FileBytes.Length;

            var rawAsset1FileName = new StringContent(Asset1.FileName);
            rawAsset1FileName.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=raw.asset1.filename");

            formDataContent.Add(rawAsset1);
            formDataContent.Add(rawAsset1FileName);
        }

        if (Asset2 != null)
        {
            var rawAsset2 = new ByteArrayContent(Asset2.FileBytes);
            rawAsset2.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=raw.asset2");
            rawAsset2.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            rawAsset2.Headers.ContentLength = Asset2.FileBytes.Length;

            var rawAsset2FileName = new StringContent(Asset2.FileName);
            rawAsset2FileName.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=raw.asset2.filename");

            formDataContent.Add(rawAsset2);
            formDataContent.Add(rawAsset2FileName);
        }

        if (Asset3 != null)
        {
            var rawAsset3 = new ByteArrayContent(Asset2.FileBytes);
            rawAsset3.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=raw.asset3");
            rawAsset3.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            rawAsset3.Headers.ContentLength = Asset2.FileBytes.Length;

            var rawAsset3FileName = new StringContent(Asset2.FileName);
            rawAsset3FileName.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=raw.asset3.filename");

            formDataContent.Add(rawAsset3);
            formDataContent.Add(rawAsset3FileName);
        }

        formDataContent.Add(rawDirectory);

        return formDataContent;
    }
}
