using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tools;
public interface IStaticAIToolDefinitionStore
{
    Task<AIToolDefinition> GetAsync([NotNull] string name);

    Task<IReadOnlyList<AIToolDefinition>> GetAllAsync();

    Task<AIToolDefinition?> GetOrNullAsync([NotNull] string name);
}
