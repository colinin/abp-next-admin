using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.PrintDesigner.CustomElements;

public interface ICustomElementRepository : IBasicRepository<CustomElement, Guid>
{
    Task<CustomElement?> FindByNameAsync(
        string name,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<CustomElement> specification,
        CancellationToken cancellationToken = default);

    Task<List<CustomElement>> GetListAsync(
        ISpecification<CustomElement> specification,
        string? sorting = nameof(CustomElement.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
