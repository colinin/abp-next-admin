using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;

public interface IFormDataRepository : IBasicRepository<FormData, Guid>
{
    Task<FormData?> FindByInstanceAsync(Guid formDefinitionId, string instanceId, CancellationToken cancellationToken = default);
}
