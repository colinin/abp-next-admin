using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace LINGYUN.Abp.WeChat.Work.Utils;

internal static class HttpContentBuildHelper
{
    public static HttpContent BuildUploadMediaContent(
        string mediaName,
        byte[] fileBytes,
        string fileName,
        string contentType = "application/octet-stream"
        )
    {
        var bytesFileName = Encoding.UTF8.GetBytes(fileName);
        var bytesHackedFileName = new char[bytesFileName.Length];
        Array.Copy(bytesFileName, 0, bytesHackedFileName, 0, bytesFileName.Length);
        var hackedFileName = new string(bytesHackedFileName);

        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentDisposition = ContentDispositionHeaderValue.Parse($"form-data; name=\"{mediaName}\"; filename=\"{hackedFileName}\"");
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
        fileContent.Headers.ContentLength = fileBytes.Length;

        var boundary = "--BOUNDARY--" + DateTimeOffset.Now.Ticks.ToString("x");
        var httpContent = new MultipartFormDataContent(boundary);
        httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"multipart/form-data; boundary={boundary}");
        httpContent.Add(fileContent);

        return httpContent;
    }
}
