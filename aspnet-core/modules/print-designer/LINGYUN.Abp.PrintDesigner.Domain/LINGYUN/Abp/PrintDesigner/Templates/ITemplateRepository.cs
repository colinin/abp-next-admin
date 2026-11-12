using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.PrintDesigner.Templates;

public interface ITemplateRepository : IBasicRepository<Template, Guid>
{
    Task<Template?> FindByNameAsync(
        string name,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<Template> specification,
        CancellationToken cancellationToken = default);

    Task<List<Template>> GetListAsync(
        ISpecification<Template> specification,
        string? sorting = nameof(Template.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
