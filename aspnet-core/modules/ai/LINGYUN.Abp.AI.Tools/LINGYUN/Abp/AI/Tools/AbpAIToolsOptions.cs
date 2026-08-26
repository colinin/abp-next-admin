using System.Collections.Generic;
using Volo.Abp.Collections;

namespace LINGYUN.Abp.AI.Tools;
public class AbpAIToolsOptions
{
    public ITypeList<IAIToolDefinitionProvider> DefinitionProviders { get; }
    public ITypeList<IAIToolProvider> AIToolProviders { get; }
    public HashSet<string> DeletedAITools { get; }
    public AbpAIToolsOptions()
    {
        DefinitionProviders = new TypeList<IAIToolDefinitionProvider>();
        AIToolProviders = new TypeList<IAIToolProvider>();
        DeletedAITools = new HashSet<string>();
    }
}
