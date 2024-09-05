using LINGYUN.Abp.TaskManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.TaskManagement;

[Authorize(TaskManagementPermissions.BackgroundJobLogs.Default)]
public class BackgroundJobLogAppService : TaskManagementApplicationService, IBackgroundJobLogAppService
{
    protected IBackgroundJobLogRepository BackgroundJobLogRepository { get; }

    public BackgroundJobLogAppService(
        IBackgroundJobLogRepository backgroundJobLogRepository)
    {
        BackgroundJobLogRepository = backgroundJobLogRepository;
    }

    [Authorize(TaskManagementPermissions.BackgroundJobLogs.Delete)]
    public virtual Task DeleteAsync(long id)
    {
        return BackgroundJobLogRepository.DeleteAsync(id);
    }

    public async virtual Task<BackgroundJobLogDto> GetAsync(long id)
    {
        var backgroundJobLog = await BackgroundJobLogRepository.GetAsync(id);

        return ObjectMapper.Map<BackgroundJobLog, BackgroundJobLogDto>(backgroundJobLog);
    }

    public async virtual Task<PagedResultDto<BackgroundJobLogDto>> GetListAsync(BackgroundJobLogGetListInput input)
    {
        var specification = new BackgroundJobLogGetListSpecification(input);

        var totalCount = await BackgroundJobLogRepository.GetCountAsync(specification);
        var backgroundJobLogs = await BackgroundJobLogRepository.GetListAsync(
            specification, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<BackgroundJobLogDto>(totalCount,
            ObjectMapper.Map<List<BackgroundJobLog>, List<BackgroundJobLogDto>>(backgroundJobLogs));
    }

    private class BackgroundJobLogGetListSpecification : Volo.Abp.Specifications.Specification<BackgroundJobLog>
    {
        protected BackgroundJobLogGetListInput Input { get; }
        public BackgroundJobLogGetListSpecification(BackgroundJobLogGetListInput input)
        {
            Input = input;
        }

        public override Expression<Func<BackgroundJobLog, bool>> ToExpression()
        {
            Expression<Func<BackgroundJobLog, bool>> expression = _ => true;

            return expression
                .AndIf(!Input.JobId.IsNullOrWhiteSpace(), x => x.JobId.Equals(Input.JobId))
                .AndIf(!Input.Type.IsNullOrWhiteSpace(), x => x.JobType.Contains(Input.Type))
                .AndIf(!Input.Group.IsNullOrWhiteSpace(), x => x.JobGroup.Equals(Input.Group))
                .AndIf(!Input.Name.IsNullOrWhiteSpace(), x => x.JobName.Equals(Input.Name))
                .AndIf(!Input.Filter.IsNullOrWhiteSpace(), x => x.JobName.Contains(Input.Filter) ||
                    x.JobGroup.Contains(Input.Filter) || x.JobType.Contains(Input.Filter) || x.Message.Contains(Input.Filter))
                .AndIf(Input.HasExceptions.HasValue, x => !string.IsNullOrWhiteSpace(x.Exception))
                .AndIf(Input.BeginRunTime.HasValue, x => x.RunTime >= Input.BeginRunTime)
                .AndIf(Input.EndRunTime.HasValue, x => x.RunTime <= Input.EndRunTime);
        }
    }
}
