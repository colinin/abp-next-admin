using LINGYUN.Abp.MultiTenancy;
using LINGYUN.Abp.Saas.Editions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.Saas.Tenants;

[Authorize(AbpSaasPermissions.Tenants.Default)]
public class TenantAppService : AbpSaasAppServiceBase, ITenantAppService
{
    protected IDistributedEventBus EventBus { get; }
    protected IEditionRepository EditionRepository { get; }
    protected ITenantRepository TenantRepository { get; }
    protected ITenantManager TenantManager { get; }

    public TenantAppService(
        ITenantRepository tenantRepository,
        IEditionRepository editionRepository,
        ITenantManager tenantManager,
        IDistributedEventBus eventBus)
    {
        EventBus = eventBus;
        TenantRepository = tenantRepository;
        EditionRepository = editionRepository;
        TenantManager = tenantManager;
    }

    public virtual async Task<TenantDto> GetAsync(Guid id)
    {
        var tenant = await TenantRepository.FindAsync(id, false);
        if (tenant == null)
        {
            throw new UserFriendlyException(L["TenantNotFoundById", id]);
        }

        var tenantDto = ObjectMapper.Map<Tenant, TenantDto>(tenant);
        if (tenant.EditionId.HasValue)
        {
            var edition = await EditionRepository.GetAsync(tenant.EditionId.Value);
            tenantDto.EditionId = edition.Id;
            tenantDto.EditionName = edition.DisplayName;
        }

        return tenantDto;
    }

    public virtual async Task<TenantDto> GetAsync(string name)
    {
        var tenant = await TenantRepository.FindByNameAsync(name, false);
        if (tenant == null)
        {
            throw new UserFriendlyException(L["TenantNotFoundByName", name]);
        }
        var tenantDto = ObjectMapper.Map<Tenant, TenantDto>(tenant);
        if (tenant.EditionId.HasValue)
        {
            var edition = await EditionRepository.GetAsync(tenant.EditionId.Value);
            tenantDto.EditionId = edition.Id;
            tenantDto.EditionName = edition.DisplayName;
        }

        return tenantDto;
    }

    public virtual async Task<PagedResultDto<TenantDto>> GetListAsync(TenantGetListInput input)
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

    [Authorize(AbpSaasPermissions.Tenants.Create)]
    public virtual async Task<TenantDto> CreateAsync(TenantCreateDto input)
    {
        var tenant = await TenantManager.CreateAsync(input.Name);
        tenant.IsActive = input.IsActive;
        tenant.EditionId = input.EditionId;
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

    [Authorize(AbpSaasPermissions.Tenants.Update)]
    public virtual async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
    {
        var tenant = await TenantRepository.GetAsync(id, false);

        await TenantManager.ChangeNameAsync(tenant, input.Name);
        tenant.IsActive = input.IsActive;
        tenant.EditionId = input.EditionId;
        input.MapExtraPropertiesTo(tenant);
        await TenantRepository.UpdateAsync(tenant);

        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    [Authorize(AbpSaasPermissions.Tenants.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var tenant = await TenantRepository.FindAsync(id);
        if (tenant == null)
        {
            return;
        }
        await TenantRepository.DeleteAsync(tenant);
    }

    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
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

    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);

        return new ListResultDto<TenantConnectionStringDto>(
            ObjectMapper.Map<List<TenantConnectionString>, List<TenantConnectionStringDto>>(tenant.ConnectionStrings.ToList()));
    }

    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task<TenantConnectionStringDto> SetConnectionStringAsync(Guid id, TenantConnectionStringCreateOrUpdate input)
    {
        var tenant = await TenantRepository.GetAsync(id);
        if (tenant.FindConnectionString(input.Name) == null)
        {
            CurrentUnitOfWork.OnCompleted(async () =>
            {
                var eventData = new ConnectionStringCreatedEventData
                {
                    TenantId = tenant.Id,
                    TenantName = tenant.Name,
                    Name = input.Name
                };

                await EventBus.PublishAsync(eventData);
            });
        }
        tenant.SetConnectionString(input.Name, input.Value);

        return new TenantConnectionStringDto
        {
            Name = input.Name,
            Value = input.Value
        };
    }

    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task DeleteConnectionStringAsync(Guid id, string name)
    {
        var tenant = await TenantRepository.GetAsync(id);

        tenant.RemoveConnectionString(name);

        CurrentUnitOfWork.OnCompleted(async () =>
        {
            var eventData = new ConnectionStringDeletedEventData
            {
                TenantId = tenant.Id,
                TenantName = tenant.Name,
                Name = name
            };

            await EventBus.PublishAsync(eventData);
        });

        await TenantRepository.UpdateAsync(tenant);
    }
}
