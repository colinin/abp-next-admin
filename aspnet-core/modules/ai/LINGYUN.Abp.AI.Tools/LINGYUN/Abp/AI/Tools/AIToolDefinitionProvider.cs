using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Tools;
public abstract class AIToolDefinitionProvider : IAIToolDefinitionProvider, ITransientDependency
{
    public abstract void Define(IAIToolDefinitionContext context);
}
