using System;
using System.Net.Http;
using System.Threading;

namespace LINGYUN.Abp.AI.Tools.Mcp;
public class McpAuthorizationCodeHandleContext
{
    public IServiceProvider ServiceProvider { get; }
    public HttpClient HttpClient { get; }
    public Uri AuthorizationUrl { get; }
    public Uri RedirectUri { get; }
    public CancellationToken CancellationToken { get; }
    public McpAuthorizationCodeHandleContext(
        IServiceProvider serviceProvider,
        HttpClient httpClient,
        Uri authorizationUrl, 
        Uri redirectUri, 
        CancellationToken cancellationToken)
    {
        ServiceProvider = serviceProvider;
        HttpClient = httpClient;
        AuthorizationUrl = authorizationUrl;
        RedirectUri = redirectUri;
        CancellationToken = cancellationToken;
    }
}
