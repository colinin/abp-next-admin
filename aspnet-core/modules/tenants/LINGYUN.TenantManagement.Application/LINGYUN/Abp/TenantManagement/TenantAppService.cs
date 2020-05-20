using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Volo.Abp.TenantManagement;

namespace LINGYUN.Abp.TenantManagement
{
    [Authorize(TenantManagementPermissions.Tenants.Default)]
    public class TenantAppService : TenantManagementAppServiceBase, ITenantAppService
    {
        protected IDataSeeder DataSeeder { get; }
        protected ITenantRepository TenantRepository { get; }
        protected ITenantManager TenantManager { get; }

        public TenantAppService(
            ITenantRepository tenantRepository,
            ITenantManager tenantManager,
            IDataSeeder dataSeeder)
        {
            DataSeeder = dataSeeder;
            TenantRepository = tenantRepository;
            TenantManager = tenantManager;
        }

        public virtual async Task<TenantDto> GetAsync(Guid id)
        {
            var tenant = await TenantRepository.GetAsync(id);
            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        public virtual async Task<PagedResultDto<TenantDto>> GetListAsync(TenantGetByPagedInputDto input)
        {
            var count = await TenantRepository.GetCountAsync(input.Filter);
            var list = await TenantRepository.GetListAsync(
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount,
                input.Filter
            );

            return new PagedResultDto<TenantDto>(
                count,
                ObjectMapper.Map<List<Tenant>, List<TenantDto>>(list)
            );
        }

        [Authorize(TenantManagementPermissions.Tenants.Create)]
        public virtual async Task<TenantDto> CreateAsync(TenantCreateDto input)
        {
            var tenant = await TenantManager.CreateAsync(input.Name);
            input.MapExtraPropertiesTo(tenant);

            await TenantRepository.InsertAsync(tenant);

            await CurrentUnitOfWork.SaveChangesAsync();

            using (CurrentTenant.Change(tenant.Id, tenant.Name))
            {
                //TODO: Handle database creation?

                await DataSeeder.SeedAsync(
                                new DataSeedContext(tenant.Id)
                                    .WithProperty("AdminEmail", input.AdminEmailAddress)
                                    .WithProperty("AdminPassword", input.AdminPassword)
                                );
            }

            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        [Authorize(TenantManagementPermissions.Tenants.Update)]
        public virtual async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
        {
            var tenant = await TenantRepository.GetAsync(id);
            await TenantManager.ChangeNameAsync(tenant, input.Name);
            input.MapExtraPropertiesTo(tenant);
            await TenantRepository.UpdateAsync(tenant);
            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        [Authorize(TenantManagementPermissions.Tenants.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var tenant = await TenantRepository.FindAsync(id);
            if (tenant == null)
            {
                return;
            }

            await TenantRepository.DeleteAsync(tenant);
        }

        [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task<TenantConnectionStringDto> GetConnectionStringAsync(TenantConnectionGetByNameInputDto tenantConnectionGetByName)
        {
            var tenant = await TenantRepository.GetAsync(tenantConnectionGetByName.Id);

            var tenantConnectionString = tenant.FindConnectionString(tenantConnectionGetByName.Name);

            return new TenantConnectionStringDto
            {
                Name = tenantConnectionGetByName.Name,
                Value = tenantConnectionString
            };
        }

        [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringAsync(Guid id)
        {
            var tenant = await TenantRepository.GetAsync(id);

            return new ListResultDto<TenantConnectionStringDto>(
                ObjectMapper.Map<List<TenantConnectionString>, List<TenantConnectionStringDto>>(tenant.ConnectionStrings));
        }

        [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task SetConnectionStringAsync(TenantConnectionStringCreateOrUpdateDto tenantConnectionStringCreateOrUpdate)
        {
            var tenant = await TenantRepository.GetAsync(tenantConnectionStringCreateOrUpdate.Id);
            tenant.SetConnectionString(tenantConnectionStringCreateOrUpdate.Name, tenantConnectionStringCreateOrUpdate.Value);
        }

        [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task DeleteConnectionStringAsync(TenantConnectionGetByNameInputDto tenantConnectionGetByName)
        {
            var tenant = await TenantRepository.GetAsync(tenantConnectionGetByName.Id);

            tenant.RemoveConnectionString(tenantConnectionGetByName.Name);
            await TenantRepository.UpdateAsync(tenant);
        }
    }
}
