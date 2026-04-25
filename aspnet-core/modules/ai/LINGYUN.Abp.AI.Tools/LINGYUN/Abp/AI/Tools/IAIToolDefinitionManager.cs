using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tools;
public interface IAIToolDefinitionManager
{
    [NotNull]
    Task<AIToolDefinition> GetAsync([NotNull] string name);

    [ItemNotNull]
    Task<IReadOnlyList<AIToolDefinition>> GetAllAsync();

    [CanBeNull]
    Task<AIToolDefinition?> GetOrNullAsync([NotNull] string name);
}
