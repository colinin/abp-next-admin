using LINGYUN.Abp.AI.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AIManagement.Tools;
public interface IAIToolDefinitionSerializer
{
    Task<AIToolDefinitionRecord[]> SerializeAsync(IEnumerable<AIToolDefinition> definitions);

    Task<AIToolDefinitionRecord> SerializeAsync(AIToolDefinition definition);
}
