using LINGYUN.Abp.Saas.Features;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.Saas.Tenants;

[Authorize(AbpSaasPermissions.Tenants.Default)]
public class TenantAppService : AbpSaasAppServiceBase, ITenantAppService
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

    public async virtual Task<TenantDto> GetAsync(Guid id)
    {
        var tenant = await TenantRepository.FindAsync(id);
        if (tenant == null)
        {
            throw new UserFriendlyException(L["TenantNotFoundById", id]);
        }

        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    public async virtual Task<TenantDto> GetAsync(string name)
    {
        var tenant = await TenantRepository.FindByNameAsync(name);
        if (tenant == null)
        {
            throw new UserFriendlyException(L["TenantNotFoundByName", name]);
        }
        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    public async virtual Task<PagedResultDto<TenantDto>> GetListAsync(TenantGetListInput input)
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
    public async virtual Task<TenantDto> CreateAsync(TenantCreateDto input)
    {
        var tenant = await TenantManager.CreateAsync(input.Name);
        tenant.IsActive = input.IsActive;
        tenant.EditionId = input.EditionId;
        tenant.SetEnableTime(input.EnableTime);
        tenant.SetDisableTime(input.DisableTime);
        input.MapExtraPropertiesTo(tenant);

        if (!input.UseSharedDatabase && !input.DefaultConnectionString.IsNullOrWhiteSpace())
        {
            tenant.SetDefaultConnectionString(input.DefaultConnectionString);
        }

        if (input.ConnectionStrings.Any())
        {
            foreach (var connectionString in input.ConnectionStrings)
            {
                tenant.SetConnectionString(connectionString.Key, connectionString.Value);
            }
        }

        await TenantRepository.InsertAsync(tenant);

        CurrentUnitOfWork.OnCompleted(async () =>
        {
            var eto = new TenantCreatedEto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Properties =
                {
                    { "AdminUserId", GuidGenerator.Create().ToString() },
                    { "AdminEmail", input.AdminEmailAddress },
                    { "AdminPassword", input.AdminPassword }
                }
            };

            //var createEventData = new CreateEventData
            //{
            //    Id = tenant.Id,
            //    Name = tenant.Name,
            //    AdminUserId = GuidGenerator.Create(),
            //    AdminEmailAddress = input.AdminEmailAddress,
            //    AdminPassword = input.AdminPassword
            //};
            ///
            await EventBus.PublishAsync(eto);
        });

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    [Authorize(AbpSaasPermissions.Tenants.Update)]
    public async virtual Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
    {
        var tenant = await TenantRepository.GetAsync(id);

        if (!string.Equals(tenant.Name, input.Name))
        {
            await TenantManager.ChangeNameAsync(tenant, input.Name);
        }

        tenant.IsActive = input.IsActive;
        tenant.EditionId = input.EditionId;
        tenant.SetEnableTime(input.EnableTime);
        tenant.SetDisableTime(input.DisableTime);
        tenant.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);
        input.MapExtraPropertiesTo(tenant);
        await TenantRepository.UpdateAsync(tenant);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Tenant, TenantDto>(tenant);
    }

    [Authorize(AbpSaasPermissions.Tenants.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var tenant = await TenantRepository.FindAsync(id);
        if (tenant == null)
        {
            return;
        }

        // 租户删除时查询会失效, 在删除前确认
        var strategy = await FeatureChecker.GetAsync(SaasFeatureNames.Tenant.RecycleStrategy, RecycleStrategy.Recycle);
        var eto = new TenantDeletedEto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Strategy = strategy,
            EntityVersion = tenant.EntityVersion,
            DefaultConnectionString = tenant.FindDefaultConnectionString(),
        };
        CurrentUnitOfWork.OnCompleted(async () =>
        {
            await EventBus.PublishAsync(eto);
        });

        await TenantRepository.DeleteAsync(tenant);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public async virtual Task<TenantConnectionStringDto> GetConnectionStringAsync(Guid id, string name)
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
    public async virtual Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);

        return new ListResultDto<TenantConnectionStringDto>(
            ObjectMapper.Map<List<TenantConnectionString>, List<TenantConnectionStringDto>>(tenant.ConnectionStrings.ToList()));
    }

    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public async virtual Task<TenantConnectionStringDto> SetConnectionStringAsync(Guid id, TenantConnectionStringCreateOrUpdate input)
    {
        var tenant = await TenantRepository.GetAsync(id);

        var oldConnectionString = tenant.FindConnectionString(input.Name);

        CurrentUnitOfWork.OnCompleted(async () =>
        {
            var eto = new TenantConnectionStringUpdatedEto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                NewValue = input.Value,
                ConnectionStringName = input.Name,
                OldValue = oldConnectionString,
            };

            await EventBus.PublishAsync(eto);
        });

        tenant.SetConnectionString(input.Name, input.Value);

        await TenantRepository.UpdateAsync(tenant);

        await CurrentUnitOfWork.SaveChangesAsync();

        return new TenantConnectionStringDto
        {
            Name = input.Name,
            Value = input.Value
        };
    }

    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public async virtual Task DeleteConnectionStringAsync(Guid id, string name)
    {
        var tenant = await TenantRepository.GetAsync(id);

        var oldConnectionString = tenant.FindConnectionString(name);

        tenant.RemoveConnectionString(name);

        CurrentUnitOfWork.OnCompleted(async () =>
        {
            var eto = new TenantConnectionStringUpdatedEto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                ConnectionStringName = name,
                OldValue = oldConnectionString,
            };

            await EventBus.PublishAsync(eto);
        });

        await TenantRepository.UpdateAsync(tenant);

        await CurrentUnitOfWork.SaveChangesAsync();
    }
}
