using LINGYUN.Abp.WorkflowManagement.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WorkflowManagement
{
    public abstract class WorkflowManagementAppServiceBase : ApplicationService
    {
        protected WorkflowManagementAppServiceBase()
        {
            LocalizationResource = typeof(WorkflowManagementResource);
            ObjectMapperContext = typeof(WorkflowManagementApplicationModule);
        }
    }
}
