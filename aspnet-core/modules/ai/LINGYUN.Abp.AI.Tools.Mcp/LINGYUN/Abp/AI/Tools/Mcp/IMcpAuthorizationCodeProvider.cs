using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tools.Mcp;
public interface IMcpAuthorizationCodeProvider
{
    Task<string?> HandleAsync(McpAuthorizationCodeHandleContext context);
}
