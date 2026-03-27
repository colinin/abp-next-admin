using System;

namespace LINGYUN.Abp.AI.Tools.Http;
public class HttpAIToolInvokeContext
{
    public IServiceProvider ServiceProvider { get; }
    public AIToolDefinition ToolDefinition { get; }
    public HttpAIToolInvokeContext(
        IServiceProvider serviceProvider,
        AIToolDefinition toolDefinition)
    {
        ServiceProvider = serviceProvider;
        ToolDefinition = toolDefinition;
    }
}
