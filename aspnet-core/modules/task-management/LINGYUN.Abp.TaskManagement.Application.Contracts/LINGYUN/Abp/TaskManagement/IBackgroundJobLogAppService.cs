using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.TaskManagement;

public interface IBackgroundJobLogAppService : 
    IReadOnlyAppService<
        BackgroundJobLogDto,
        long,
        BackgroundJobLogGetListInput>,
    IDeleteAppService<long>
{
}
