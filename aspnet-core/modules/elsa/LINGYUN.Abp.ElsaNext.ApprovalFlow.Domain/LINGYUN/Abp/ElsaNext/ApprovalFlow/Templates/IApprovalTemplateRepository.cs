using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;

public interface IApprovalTemplateRepository : IBasicRepository<ApprovalTemplate, Guid>
{
	Task<ApprovalTemplate?> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<ApprovalTemplate> specification,
        CancellationToken cancellationToken = default);

    Task<List<ApprovalTemplate>> GetListAsync(
        ISpecification<ApprovalTemplate> specification,
        string sorting = $"{nameof(ApprovalTemplate.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
