using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LY.MicroService.Applications.Single.Controllers;

[Route("api/forwarded-headers")]
public class ForwardedHeadersController : AbpControllerBase
{
    private readonly ForwardedHeadersOptions _options;

    public ForwardedHeadersController(IOptionsMonitor<ForwardedHeadersOptions> options)
    {
        _options = options.CurrentValue;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var headerOptions = new Dictionary<string, string>
        {
            { "ForwardedHeaders", _options.ForwardedHeaders.ToString() },
            { "ForwardedForHeaderName", _options.ForwardedForHeaderName },
            { "ForwardedHostHeaderName", _options.ForwardedHostHeaderName },
            { "ForwardedPrefixHeaderName", _options.ForwardedPrefixHeaderName },
            { "ForwardedProtoHeaderName", _options.ForwardedProtoHeaderName },
            { "ForwardLimit", _options.ForwardLimit?.ToString() },
            { "KnownProxies", _options.KnownProxies?.Select(x => x.ToString()).JoinAsString(";") },
            { "KnownIPNetworks", _options.KnownIPNetworks?.Select(x => x.ToString()).JoinAsString(";") },
            { "AllowedHosts", _options.AllowedHosts?.JoinAsString(";") },
            { "RequireHeaderSymmetry", _options.RequireHeaderSymmetry.ToString() },
        };

        var request = new Dictionary<string, string>
        {
            { "Scheme", Request.Scheme },
            { "Host", Request.Host.ToString() },
            { "Path", Request.Path.ToString() },
            { "Protocol", Request.Protocol },
            { "QueryString", Request.QueryString.ToString() },
        };

        var headers = new Dictionary<string, string>();
        foreach (var header in Request.Headers)
        {
            headers[header.Key] = header.Value;
        }

        return new JsonResult(new
        {
            headerOptions,
            request,
            headers
        }, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        });
    }
}
