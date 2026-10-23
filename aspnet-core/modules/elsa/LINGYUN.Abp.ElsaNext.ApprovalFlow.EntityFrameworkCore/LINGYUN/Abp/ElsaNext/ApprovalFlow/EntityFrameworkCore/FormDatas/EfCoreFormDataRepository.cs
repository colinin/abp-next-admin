using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.FormDatas;

public class EfCoreFormDataRepository : EfCoreRepository<IElsaNextApprovalFlowDbContext, FormData, Guid>, IFormDataRepository
{
	public EfCoreFormDataRepository(IDbContextProvider<IElsaNextApprovalFlowDbContext> dbContextProvider)
		: base(dbContextProvider)
	{
	}

	public async virtual Task<FormData?> FindByInstanceAsync(Guid formDefinitionId, string instanceId, CancellationToken cancellationToken = default)
	{
		return await (await GetQueryableAsync())
            .Where(x => x.FormDefinitionId == formDefinitionId && x.InstanceId == instanceId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
	}
}
