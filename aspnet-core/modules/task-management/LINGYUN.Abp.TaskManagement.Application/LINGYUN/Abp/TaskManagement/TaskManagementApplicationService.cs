using LINGYUN.Abp.TaskManagement.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.TaskManagement;

public abstract class TaskManagementApplicationService : ApplicationService
{
    protected TaskManagementApplicationService()
    {
        LocalizationResource = typeof(TaskManagementResource);
        ObjectMapperContext = typeof(TaskManagementApplicationModule);
    }
}
