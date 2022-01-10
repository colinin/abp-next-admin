using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.TaskManagement;

public interface IBackgroundJobInfoAppService :
    ICrudAppService<
        BackgroundJobInfoDto,
        Guid,
        BackgroundJobInfoGetListInput,
        BackgroundJobInfoCreateDto,
        BackgroundJobInfoUpdateDto>
{
    Task TriggerAsync(Guid id);

    Task PauseAsync(Guid id);

    Task ResumeAsync(Guid id);

    Task StopAsync(Guid id);
}
