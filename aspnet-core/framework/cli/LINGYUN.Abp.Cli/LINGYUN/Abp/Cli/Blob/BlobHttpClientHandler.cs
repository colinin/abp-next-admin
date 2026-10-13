using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Cli.Blob;

public class BlobHttpClientHandler : DelegatingHandler
{
    protected ICurrentBlob CurrentBlob { get; }
    public BlobHttpClientHandler(ICurrentBlob currentBlob)
    {
        CurrentBlob = currentBlob;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!CurrentBlob.BaseUrl.IsNullOrWhiteSpace() && request.RequestUri != null)
        {
            if (request.RequestUri is { } original)
            {
                var pathAndQuery = original.IsAbsoluteUri
                    ? original.PathAndQuery
                    : original.ToString();

                var builder = new UriBuilder(CurrentBlob.BaseUrl)
                {
                    Path = original.IsAbsoluteUri ? original.AbsolutePath : pathAndQuery.Split('?')[0],
                    Query = original.IsAbsoluteUri ? original.Query.TrimStart('?') : ""
                };
                request.RequestUri = builder.Uri;
            }
        }
        if (!CurrentBlob.TenantId.IsNullOrWhiteSpace())
        {
            request.Headers.TryAddWithoutValidation("__tenant", CurrentBlob.TenantId);
        }

        request.Headers.TryAddWithoutValidation("_AbpDontWrapResult", bool.TrueString);

        return base.SendAsync(request, cancellationToken);
    }
}
