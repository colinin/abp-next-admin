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

            if (!input.UseSharedDatabase && !input.DefaultConnectionString.IsNullOrWhiteSpace())
            {
                tenant.SetDefaultConnectionString(input.DefaultConnectionString);
            }

            await TenantRepository.InsertAsync(tenant);

            CurrentUnitOfWork.OnCompleted(async () =>
            {
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
            });

            return ObjectMapper.Map<Tenant, TenantDto>(tenant);
        }

        [Authorize(TenantManagementPermissions.Tenants.Update)]
        public virtual async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
        {
            var tenant = await TenantRepository.GetAsync(id, false);

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
            if (tenant.FindConnectionString(tenantConnectionStringCreateOrUpdate.Name) == null)
            {
                CurrentUnitOfWork.OnCompleted(async () =>
                {
                    var eventData = new ConnectionStringCreatedEventData
                    {
                        Id = tenant.Id,
                        Name = tenantConnectionStringCreateOrUpdate.Name
                    };

                    await EventBus.PublishAsync(eventData);
                });
            }
            tenant.SetConnectionString(tenantConnectionStringCreateOrUpdate.Name, tenantConnectionStringCreateOrUpdate.Value);

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

            var eventData = new ConnectionStringDeletedEventData
            {
                Id = tenant.Id,
                Name = name
            };
            await EventBus.PublishAsync(eventData);

            await TenantRepository.UpdateAsync(tenant);
        }
    }
}
