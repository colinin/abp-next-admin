using LINGYUN.Abp.DataProtectionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.DataProtectionManagement;

[Controller]
[Authorize(DataProtectionManagementPermissionNames.EntityTypeInfo.Default)]
[RemoteService(Name = DataProtectionManagementRemoteServiceConsts.RemoteServiceName)]
[Area(DataProtectionManagementRemoteServiceConsts.ModuleName)]
[Route($"api/{DataProtectionManagementRemoteServiceConsts.ModuleName}/entity-type-infos")]
public class EntityTypeInfoController : AbpControllerBase, IEntityTypeInfoAppService
{
    private readonly IEntityTypeInfoAppService _service;

    public EntityTypeInfoController(IEntityTypeInfoAppService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<EntityTypeInfoDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<EntityTypeInfoDto>> GetListAsync(GetEntityTypeInfoListInput input)
    {
        return _service.GetListAsync(input);
    }
}
