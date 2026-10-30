using LINGYUN.Abp.ElsaNext.Email.Contracts;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.Email.Services;

/// <summary>
/// Download files from the Internet.
/// </summary>
public class DefaultDownloader : IDownloader
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Constructor.
    /// </summary>
    public DefaultDownloader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Download whatever is returned at the specified URL.
    /// </summary>
    public async Task<HttpResponseMessage> DownloadAsync(Uri url, CancellationToken cancellationToken = default) => await _httpClient.GetAsync(url, cancellationToken);
}
