using LINGYUN.Abp.DataProtection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.DataProtectionManagement;
public interface IOrganizationUnitEntityRuleRepository : IBasicRepository<OrganizationUnitEntityRule, Guid>
{
    Task<OrganizationUnitEntityRule> FindEntityRuleAsync(
        string orgCode,
        string entityTypeFullName,
        DataAccessOperation operation = DataAccessOperation.Read,
        CancellationToken cancellationToken = default);

    Task<List<OrganizationUnitEntityRule>> GetListByEntityAsync(
        string entityTypeFullName,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<OrganizationUnitEntityRule> specification,
        CancellationToken cancellationToken = default);

    Task<List<OrganizationUnitEntityRule>> GetCountAsync(
        ISpecification<OrganizationUnitEntityRule> specification,
        string sorting = nameof(OrganizationUnitEntityRule.EntityTypeFullName),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
