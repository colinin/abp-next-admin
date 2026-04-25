using System.Threading.Tasks;
using System.Web;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tools.Mcp;

[Dependency(TryRegister = true)]
public class DefaultMcpAuthorizationCodeProvider : IMcpAuthorizationCodeProvider, ISingletonDependency
{
    public async virtual Task<string?> HandleAsync(McpAuthorizationCodeHandleContext context)
    {
        using var redirectResponse = await context.HttpClient.GetAsync(context.AuthorizationUrl, context.CancellationToken);
        var location = redirectResponse.Headers.Location;

        if (location is not null && !string.IsNullOrEmpty(location.Query))
        {
            // Parse query string to extract "code" parameter
            var query = location.Query.TrimStart('?');
            foreach (var pair in query.Split('&'))
            {
                var parts = pair.Split('=', 2);
                if (parts.Length == 2 && parts[0] == "code")
                {
                    return HttpUtility.UrlDecode(parts[1]);
                }
            }
        }

        return null;
    }
}
