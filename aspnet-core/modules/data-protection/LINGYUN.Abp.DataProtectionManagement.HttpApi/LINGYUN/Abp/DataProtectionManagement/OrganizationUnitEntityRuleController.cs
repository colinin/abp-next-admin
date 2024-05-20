using LINGYUN.Abp.DataProtectionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.DataProtectionManagement;

[Controller]
[Authorize(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Default)]
[RemoteService(Name = DataProtectionManagementRemoteServiceConsts.RemoteServiceName)]
[Area(DataProtectionManagementRemoteServiceConsts.ModuleName)]
[Route($"api/{DataProtectionManagementRemoteServiceConsts.ModuleName}/entity-rule/organization-units")]
public class OrganizationUnitEntityRuleController : AbpControllerBase, IOrganizationUnitEntityRuleAppService
{
    private readonly IOrganizationUnitEntityRuleAppService _service;

    public OrganizationUnitEntityRuleController(IOrganizationUnitEntityRuleAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<OrganizationUnitEntityRuleDto> GetAsync(OrganizationUnitEntityRuleGetInput input)
    {
        return _service.GetAsync(input);
    }

    [HttpPost]
    [Authorize(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Create)]
    public virtual Task<OrganizationUnitEntityRuleDto> CreateAsync(OrganizationUnitEntityRuleCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(DataProtectionManagementPermissionNames.OrganizationUnitEntityRule.Update)]
    public virtual Task<OrganizationUnitEntityRuleDto> UpdateAsync(Guid id, OrganizationUnitEntityRuleUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }
}
