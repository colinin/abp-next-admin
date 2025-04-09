using LINGYUN.Abp.DataProtectionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.DataProtectionManagement;

[Controller]
[Authorize(DataProtectionManagementPermissionNames.SubjectStrategy.Default)]
[RemoteService(Name = DataProtectionManagementRemoteServiceConsts.RemoteServiceName)]
[Area(DataProtectionManagementRemoteServiceConsts.ModuleName)]
[Route($"api/{DataProtectionManagementRemoteServiceConsts.ModuleName}/subject-strategys")]
public class SubjectStrategyController : AbpControllerBase, ISubjectStrategyAppService
{
    private readonly ISubjectStrategyAppService _service;
    public SubjectStrategyController(ISubjectStrategyAppService service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual Task<SubjectStrategyDto> GetAsync(SubjectStrategyGetInput input)
    {
        return _service.GetAsync(input);
    }

    [HttpPut]
    [Authorize(DataProtectionManagementPermissionNames.SubjectStrategy.Change)]
    public virtual Task<SubjectStrategyDto> SetAsync(SubjectStrategySetInput input)
    {
        return _service.SetAsync(input);
    }
}
