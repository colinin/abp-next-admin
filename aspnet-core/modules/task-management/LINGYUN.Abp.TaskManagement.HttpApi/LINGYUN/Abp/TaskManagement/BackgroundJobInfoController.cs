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
    public Task DeleteAsync(string id)
    {
        return BackgroundJobInfoAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<BackgroundJobInfoDto> GetAsync(string id)
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
    public Task PauseAsync(string id)
    {
        return BackgroundJobInfoAppService.PauseAsync(id);
    }

    [HttpPut]
    [Route("{id}/resume")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Resume)]
    public Task ResumeAsync(string id)
    {
        return BackgroundJobInfoAppService.ResumeAsync(id);
    }

    [HttpPut]
    [Route("{id}/trigger")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Trigger)]
    public Task TriggerAsync(string id)
    {
        return BackgroundJobInfoAppService.TriggerAsync(id);
    }

    [HttpPut]
    [Route("{id}/stop")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Stop)]
    public Task StopAsync(string id)
    {
        return BackgroundJobInfoAppService.StopAsync(id);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Update)]
    public Task<BackgroundJobInfoDto> UpdateAsync(string id, BackgroundJobInfoUpdateDto input)
    {
        return BackgroundJobInfoAppService.UpdateAsync(id, input);
    }

    [HttpPut]
    [Route("{id}/start")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Start)]
    public Task StartAsync(string id)
    {
        return BackgroundJobInfoAppService.StartAsync(id);
    }

    [HttpPut]
    [Route("bulk-stop")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Stop)]
    public Task BulkStopAsync(BackgroundJobInfoBatchInput input)
    {
        return BackgroundJobInfoAppService.BulkStopAsync(input);
    }

    [HttpPut]
    [Route("bulk-start")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Start)]
    public Task BulkStartAsync(BackgroundJobInfoBatchInput input)
    {
        return BackgroundJobInfoAppService.BulkStartAsync(input);
    }

    [HttpPut]
    [Route("bulk-trigger")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Trigger)]
    public Task BulkTriggerAsync(BackgroundJobInfoBatchInput input)
    {
        return BackgroundJobInfoAppService.BulkTriggerAsync(input);
    }

    [HttpPut]
    [Route("bulk-resume")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Resume)]
    public Task BulkResumeAsync(BackgroundJobInfoBatchInput input)
    {
        return BackgroundJobInfoAppService.BulkResumeAsync(input);
    }

    [HttpPut]
    [Route("bulk-pause")]
    [Authorize(TaskManagementPermissions.BackgroundJobs.Pause)]
    public Task BulkPauseAsync(BackgroundJobInfoBatchInput input)
    {
        return BackgroundJobInfoAppService.BulkPauseAsync(input);
    }

    [HttpPut]
    [Route("bulk-delete")]
    public Task BulkDeleteAsync(BackgroundJobInfoBatchInput input)
    {
        return BackgroundJobInfoAppService.BulkDeleteAsync(input);
    }
}
