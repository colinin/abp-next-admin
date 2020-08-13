using LINGYUN.Abp.FileManagement.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.FileManagement
{
    public class FileManagementApplicationServiceBase : ApplicationService
    {
        protected FileManagementApplicationServiceBase()
        {
            LocalizationResource = typeof(AbpFileManagementResource);
        }
    }
}
