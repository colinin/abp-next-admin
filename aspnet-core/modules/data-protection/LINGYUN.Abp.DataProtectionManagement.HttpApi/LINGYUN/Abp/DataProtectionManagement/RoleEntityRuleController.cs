using LINGYUN.Abp.DataProtectionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.DataProtectionManagement;

[Controller]
[Authorize(DataProtectionManagementPermissionNames.RoleEntityRule.Default)]
[RemoteService(Name = DataProtectionManagementRemoteServiceConsts.RemoteServiceName)]
[Area(DataProtectionManagementRemoteServiceConsts.ModuleName)]
[Route($"api/{DataProtectionManagementRemoteServiceConsts.ModuleName}/entity-rule/roles")]
public class RoleEntityRuleController : AbpControllerBase, IRoleEntityRuleAppService
{
    private readonly IRoleEntityRuleAppService _service;

    public RoleEntityRuleController(IRoleEntityRuleAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<RoleEntityRuleDto> GetAsync(RoleEntityRuleGetInput input)
    {
        return _service.GetAsync(input);
    }

    [HttpPost]
    [Authorize(DataProtectionManagementPermissionNames.RoleEntityRule.Create)]
    public virtual Task<RoleEntityRuleDto> CreateAsync(RoleEntityRuleCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(DataProtectionManagementPermissionNames.RoleEntityRule.Update)]
    public virtual Task<RoleEntityRuleDto> UpdateAsync(Guid id, RoleEntityRuleUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }
}
