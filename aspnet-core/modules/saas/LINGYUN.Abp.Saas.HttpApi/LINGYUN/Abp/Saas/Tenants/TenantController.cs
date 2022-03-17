using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Saas.Tenants;

[Controller]
[Authorize(AbpSaasPermissions.Tenants.Default)]
[RemoteService(Name = AbpSaasRemoteServiceConsts.RemoteServiceName)]
[Area(AbpSaasRemoteServiceConsts.ModuleName)]
[Route("api/saas/tenants")]
public class TenantController : AbpSaasControllerBase, ITenantAppService
{
    protected ITenantAppService TenantAppService { get; }

    public TenantController(ITenantAppService tenantAppService)
    {
        TenantAppService = tenantAppService;
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<TenantDto> GetAsync(Guid id)
    {
        return TenantAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("by-name/{name}")]
    public virtual Task<TenantDto> GetAsync(string name)
    {
        return TenantAppService.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<TenantDto>> GetListAsync(TenantGetListInput input)
    {
        return TenantAppService.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(AbpSaasPermissions.Tenants.Create)]
    public virtual Task<TenantDto> CreateAsync(TenantCreateDto input)
    {
        return TenantAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(AbpSaasPermissions.Tenants.Update)]
    public virtual Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
    {
        return TenantAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(AbpSaasPermissions.Tenants.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return TenantAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}/connection-string/{name}")]
    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public virtual Task<TenantConnectionStringDto> GetConnectionStringAsync(Guid id, string name)
    {
        return TenantAppService.GetConnectionStringAsync(id, name);
    }

    [HttpGet]
    [Route("{id}/connection-string")]
    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public virtual Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringAsync(Guid id)
    {
        return TenantAppService.GetConnectionStringAsync(id);
    }

    [HttpPut]
    [Route("{id}/connection-string")]
    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public virtual Task<TenantConnectionStringDto> SetConnectionStringAsync(Guid id, TenantConnectionStringCreateOrUpdate input)
    {
        return TenantAppService.SetConnectionStringAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}/connection-string/{name}")]
    [Authorize(AbpSaasPermissions.Tenants.ManageConnectionStrings)]
    public virtual Task DeleteConnectionStringAsync(Guid id, string name)
    {
        return TenantAppService.DeleteConnectionStringAsync(id, name);
    }
}
