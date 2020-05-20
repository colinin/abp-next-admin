using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.TenantManagement
{
    public interface ITenantAppService : ICrudAppService<TenantDto, Guid, TenantGetByPagedInputDto, TenantCreateDto, TenantUpdateDto>
    {
        Task<TenantConnectionStringDto> GetConnectionStringAsync(TenantConnectionGetByNameInputDto tenantConnectionGetByName);

        Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringAsync(Guid id);

        Task SetConnectionStringAsync(TenantConnectionStringCreateOrUpdateDto tenantConnectionStringCreateOrUpdate);

        Task DeleteConnectionStringAsync(TenantConnectionGetByNameInputDto tenantConnectionGetByName);
    }
}
