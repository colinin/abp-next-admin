using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.TenantManagement
{
    public interface ITenantAppService : ICrudAppService<TenantDto, Guid, TenantGetByPagedInputDto, TenantCreateDto, TenantUpdateDto>
    {
        Task<TenantDto> GetAsync(
            [Required]
            string name);

        Task<TenantConnectionStringDto> GetConnectionStringAsync(
            Guid id,
            [Required]
            string connectionName);

        Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringAsync(Guid id);

        Task<TenantConnectionStringDto> SetConnectionStringAsync(Guid id, TenantConnectionStringCreateOrUpdateDto tenantConnectionStringCreateOrUpdate);

        Task DeleteConnectionStringAsync(
            Guid id,
            [Required]
            string connectionName);
    }
}
