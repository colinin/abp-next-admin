using LINGYUN.Abp.TaskManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.TaskManagement;

public abstract class TaskManagementController : AbpControllerBase
{
    protected TaskManagementController()
    {
        LocalizationResource = typeof(TaskManagementResource);
    }
}
