using LINGYUN.Abp.MultiTenancy;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectExtending;
using Volo.Abp.TenantManagement;

namespace LINGYUN.Abp.TenantManagement
{
    [Authorize(TenantManagementPermissions.Tenants.Default)]
    public class TenantAppService : TenantManagementAppServiceBase, ITenantAppService
    {
        protected IDistributedEventBus EventBus { get; }
        protected ITenantRepository TenantRepository { get; }
        protected ITenantManager TenantManager { get; }

        public TenantAppService(
            ITenantRepository tenantRepository,
            ITenantManager tenantManager,
            IDistributedEventBus eventBus)
        {
            EventBus = eventBus;
            TenantRepository = tenantRepository;
            TenantManager = tenantManager;
        }

        public virtual async Task<TenantDto> GetAsync(Guid id)
        {
            var tenant = await TenantRepository.FindAsync(id, false);
            if(tenant == null)
            {
                throw new UserFriendlyException(L["TenantNotFoundById", id]);
            }
            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        public virtual async Task<TenantDto> GetAsync(string name)
        {
            var tenant = await TenantRepository.FindByNameAsync(name, false);
            if (tenant == null)
            {
                throw new UserFriendlyException(L["TenantNotFoundByName", name]);
            }
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

            var createEventData = new CreateEventData
            {
                Id = tenant.Id,
                Name = tenant.Name,
                AdminUserId = GuidGenerator.Create(),
                AdminEmailAddress = input.AdminEmailAddress,
                AdminPassword = input.AdminPassword
            };
            // 因为项目各自独立，租户增加时添加管理用户必须通过事件总线
            // 而 TenantEto 对象没有包含所需的用户名密码，需要独立发布事件
            await EventBus.PublishAsync(createEventData);

            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        [Authorize(TenantManagementPermissions.Tenants.Update)]
        public virtual async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
        {
            var tenant = await TenantRepository.GetAsync(id, false);
            var updateEventData = new UpdateEventData
            {
                Id = tenant.Id,
                OriginName = tenant.Name,
                Name = input.Name
            };
            await TenantManager.ChangeNameAsync(tenant, input.Name);
            input.MapExtraPropertiesTo(tenant);
            await TenantRepository.UpdateAsync(tenant);

            await EventBus.PublishAsync(updateEventData);
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
        public virtual async Task<TenantConnectionStringDto> GetConnectionStringAsync(Guid id, string name)
        {
            var tenant = await TenantRepository.GetAsync(id);

            var tenantConnectionString = tenant.FindConnectionString(name);

            return new TenantConnectionStringDto
            {
                Name = name,
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
        public virtual async Task<TenantConnectionStringDto> SetConnectionStringAsync(Guid id, TenantConnectionStringCreateOrUpdateDto tenantConnectionStringCreateOrUpdate)
        {
            var tenant = await TenantRepository.GetAsync(id);
            tenant.SetConnectionString(tenantConnectionStringCreateOrUpdate.Name, tenantConnectionStringCreateOrUpdate.Value);
            var updateEventData = new UpdateEventData
            {
                Id = tenant.Id,
                OriginName = tenant.Name,
                Name = tenant.Name
            };
            // abp当前版本(3.0.0)在EntityChangeEventHelper中存在一个问题,无法发送框架默认的Eto,预计3.1.0修复
            // 发送自定义的事件数据来确保缓存被更新
            await EventBus.PublishAsync(updateEventData);

            return new TenantConnectionStringDto
            {
                Name = tenantConnectionStringCreateOrUpdate.Name,
                Value = tenantConnectionStringCreateOrUpdate.Value
            };
        }

        [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
        public virtual async Task DeleteConnectionStringAsync(Guid id, string name)
        {
            var tenant = await TenantRepository.GetAsync(id);

            tenant.RemoveConnectionString(name);

            var updateEventData = new UpdateEventData
            {
                Id = tenant.Id,
                OriginName = tenant.Name,
                Name = tenant.Name
            };
            await EventBus.PublishAsync(updateEventData);

            await TenantRepository.UpdateAsync(tenant);
        }
    }
}
