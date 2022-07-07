using LINGYUN.Abp.BackgroundTasks.Activities;
using LINGYUN.Abp.TaskManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TaskManagement;

[Authorize(TaskManagementPermissions.BackgroundJobs.ManageActions)]
public class BackgroundJobActionAppService : TaskManagementApplicationService, IBackgroundJobActionAppService
{
    protected IJobActionDefinitionManager JobActionDefinitionManager { get; }
    protected IBackgroundJobInfoRepository BackgroundJobInfoRepository { get; }
    protected IBackgroundJobActionRepository BackgroundJobActionRepository { get; }
    
    public BackgroundJobActionAppService(
        IJobActionDefinitionManager jobActionDefinitionManager,
        IBackgroundJobInfoRepository backgroundJobInfoRepository,
        IBackgroundJobActionRepository backgroundJobActionRepository)
    {
        JobActionDefinitionManager = jobActionDefinitionManager;
        BackgroundJobInfoRepository = backgroundJobInfoRepository;
        BackgroundJobActionRepository = backgroundJobActionRepository;
    }

    public async virtual Task<BackgroundJobActionDto> AddActionAsync(string jobId, BackgroundJobActionCreateDto input)
    {
        var job = await BackgroundJobInfoRepository.GetAsync(jobId);

        var action = new BackgroundJobAction(
            GuidGenerator.Create(),
            job.Id,
            input.Name,
            input.Paramters,
            CurrentTenant.Id)
        {
            IsEnabled = input.IsEnabled,
        };

        action = await BackgroundJobActionRepository.InsertAsync(action);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<BackgroundJobAction, BackgroundJobActionDto>(action);
    }

    public async virtual Task DeleteActionAsync(Guid id)
    {
        var action = await BackgroundJobActionRepository.GetAsync(id);

        await BackgroundJobActionRepository.DeleteAsync(action);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<ListResultDto<BackgroundJobActionDto>> GetActionsAsync(string jobId)
    {
        var actions = await BackgroundJobActionRepository.GetListAsync(jobId);

        return new ListResultDto<BackgroundJobActionDto>(
            ObjectMapper.Map<List<BackgroundJobAction>, List<BackgroundJobActionDto>>(actions));
    }

    public virtual Task<ListResultDto<BackgroundJobActionDefinitionDto>> GetDefinitionsAsync(BackgroundJobActionGetDefinitionsInput input)
    {
        var actionDefinitions = JobActionDefinitionManager.GetAll();


        var dtoList = actionDefinitions
            .WhereIf(input.Type.HasValue, action => action.Type == input.Type.Value)
            .Select(action => new BackgroundJobActionDefinitionDto
            {
                Name = action.Name,
                Type = action.Type,
                DisplayName = action.DisplayName.Localize(StringLocalizerFactory),
                Description = action.Description?.Localize(StringLocalizerFactory),
                Paramters = action.Paramters.Select(p => new BackgroundJobActionParamterDto
                {
                    Name = p.Name,
                    Required = p.Required,
                    DisplayName = p.DisplayName.Localize(StringLocalizerFactory),
                    Description = p.Description?.Localize(StringLocalizerFactory),
                }).ToList(),
            }).ToList();

        return Task.FromResult(new ListResultDto<BackgroundJobActionDefinitionDto>(dtoList));
    }

    public async virtual Task<BackgroundJobActionDto> UpdateActionAsync(Guid id, BackgroundJobActionUpdateDto input)
    {
        var action = await BackgroundJobActionRepository.GetAsync(id);

        action.IsEnabled = input.IsEnabled;
        action.Paramters = input.Paramters;

        action = await BackgroundJobActionRepository.UpdateAsync(action);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<BackgroundJobAction, BackgroundJobActionDto>(action);
    }
}
