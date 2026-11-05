using System;
using System.Threading.Tasks;
using LINGYUN.Abp.OperationRateLimiting.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.OperationRateLimiting;

[Area("OperationRateLimiting")]
[Route("api/operation-rate-limiting/policies")]
[RemoteService(Name = AbpOperationRateLimitingRemoteServiceConsts.RemoteServiceName)]
public class OperationRateLimitingPolicyController :
    AbpControllerBase,
    IOperationRateLimitingPolicyAppService
{
    private readonly IOperationRateLimitingPolicyAppService _policyAppService;

    public OperationRateLimitingPolicyController(IOperationRateLimitingPolicyAppService policyAppService)
    {
        _policyAppService = policyAppService;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public virtual Task<OperationRateLimitingPolicyDto> GetAsync(Guid id)
    {
        return _policyAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("by-name/{name}")]
    public virtual Task<OperationRateLimitingPolicyDto> GetByNameAsync(string name)
    {
        return _policyAppService.GetByNameAsync(name);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<OperationRateLimitingPolicyDto>> GetListAsync(
        [FromQuery] OperationRateLimitingPolicyGetListInput input)
    {
        return _policyAppService.GetListAsync(input);
    }

    [HttpPost]
    public virtual Task<OperationRateLimitingPolicyDto> CreateAsync(
        [FromBody] CreateOperationRateLimitingPolicyDto input)
    {
        return _policyAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public virtual Task<OperationRateLimitingPolicyDto> UpdateAsync(
        Guid id,
        [FromBody] UpdateOperationRateLimitingPolicyDto input)
    {
        return _policyAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _policyAppService.DeleteAsync(id);
    }
}
