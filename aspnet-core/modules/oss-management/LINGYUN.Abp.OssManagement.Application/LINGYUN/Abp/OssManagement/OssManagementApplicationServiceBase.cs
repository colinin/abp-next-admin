using LINGYUN.Abp.OssManagement.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OssManagement
{
    public class OssManagementApplicationServiceBase : ApplicationService
    {
        protected OssManagementApplicationServiceBase()
        {
            LocalizationResource = typeof(AbpOssManagementResource);
            ObjectMapperContext = typeof(AbpOssManagementApplicationModule);
        }
    }
}
