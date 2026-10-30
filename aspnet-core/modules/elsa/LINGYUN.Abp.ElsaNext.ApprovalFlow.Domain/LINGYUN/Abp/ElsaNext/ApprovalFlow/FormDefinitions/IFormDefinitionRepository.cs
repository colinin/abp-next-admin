using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;

public interface IFormDefinitionRepository : IBasicRepository<FormDefinition, Guid>
{
	Task<FormDefinition?> FindByCodeAsync(
        string code, 
        int? version = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<FormDefinition> specification,
        CancellationToken cancellationToken = default);

    Task<List<FormDefinition>> GetListAsync(
        ISpecification<FormDefinition> specification,
        string? sorting = $"{nameof(FormDefinition.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
