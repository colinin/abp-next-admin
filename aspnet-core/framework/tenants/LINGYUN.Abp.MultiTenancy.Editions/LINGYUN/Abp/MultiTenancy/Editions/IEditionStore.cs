using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MultiTenancy.Editions;

public interface IEditionStore
{
    Task<EditionInfo> FindByTenantAsync(Guid tenantId);
}
