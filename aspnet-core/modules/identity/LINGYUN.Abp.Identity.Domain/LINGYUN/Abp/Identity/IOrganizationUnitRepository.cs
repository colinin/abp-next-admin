using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Identity;

public interface IOrganizationUnitRepository : Volo.Abp.Identity.IOrganizationUnitRepository
{
    Task<int> GetCountAsync(
        ISpecification<OrganizationUnit> specification,
        CancellationToken cancellationToken = default);

    Task<List<OrganizationUnit>> GetListAsync(
        ISpecification<OrganizationUnit> specification,
        string sorting = nameof(OrganizationUnit.Code),
        int maxResultCount = 10,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}
