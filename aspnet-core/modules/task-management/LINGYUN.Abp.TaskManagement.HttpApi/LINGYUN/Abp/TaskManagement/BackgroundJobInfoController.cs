using LINGYUN.Abp.TaskManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TaskManagement;

[RemoteService(Name = TaskManagementRemoteServiceConsts.RemoteServiceName)]
[Area(TaskManagementRemoteServiceConsts.ModuleName)]
[Authorize(TaskManagementPermissions.BackgroundJobs.Default)]
[Route($"api/{TaskManagementRemoteServiceConsts.ModuleName}/background-jobs")]
public class BackgroundJobInfoController : TaskManagementController, IBackgroundJobInfoAppService
{
    protected IBackgroundJobInfoAppService BackgroundJobInfoAppService { get; }

    public BackgroundJobInfoController(
        IBackgroundJobInfoAppService backgroundJobInfoAppService)
    {
        BackgroundJobInfoAppService = backgroundJobInfoAppService;
    }

    [HttpPost]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Create)]
    public Task<BackgroundJobInfoDto> CreateAsync(BackgroundJobInfoCreateDto input)
    {
        return BackgroundJobInfoAppService.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Delete)]
    public Task DeleteAsync(Guid id)
    {
        return BackgroundJobInfoAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<BackgroundJobInfoDto> GetAsync(Guid id)
    {
        return BackgroundJobInfoAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<BackgroundJobInfoDto>> GetListAsync(BackgroundJobInfoGetListInput input)
    {
        return BackgroundJobInfoAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}/pause")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Pause)]
    public Task PauseAsync(Guid id)
    {
        return BackgroundJobInfoAppService.PauseAsync(id);
    }

    [HttpPut]
    [Route("{id}/resume")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Resume)]
    public Task ResumeAsync(Guid id)
    {
        return BackgroundJobInfoAppService.ResumeAsync(id);
    }

    [HttpPut]
    [Route("{id}/trigger")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Trigger)]
    public Task TriggerAsync(Guid id)
    {
        return BackgroundJobInfoAppService.TriggerAsync(id);
    }

    [HttpPut]
    [Route("{id}/stop")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Stop)]
    public Task StopAsync(Guid id)
    {
        return BackgroundJobInfoAppService.StopAsync(id);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Update)]
    public Task<BackgroundJobInfoDto> UpdateAsync(Guid id, BackgroundJobInfoUpdateDto input)
    {
        return BackgroundJobInfoAppService.UpdateAsync(id, input);
    }
}
