using Volo.Abp.Collections;

namespace LINGYUN.Abp.AI.Tools;
public class AbpAIToolsOptions
{
    public ITypeList<IAIToolDefinitionProvider> DefinitionProviders { get; }
    public ITypeList<IAIToolProvider> AIToolProviders { get; }
    public AbpAIToolsOptions()
    {
        DefinitionProviders = new TypeList<IAIToolDefinitionProvider>();
        AIToolProviders = new TypeList<IAIToolProvider>();
    }
}
