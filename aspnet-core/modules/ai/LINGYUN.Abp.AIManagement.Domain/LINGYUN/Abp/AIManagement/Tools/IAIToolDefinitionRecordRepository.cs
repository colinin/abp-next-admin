using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.AIManagement.Tools;
public interface IAIToolDefinitionRecordRepository : IRepository<AIToolDefinitionRecord, Guid>
{
    Task<AIToolDefinitionRecord?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}
