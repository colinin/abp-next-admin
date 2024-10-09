using LINGYUN.Abp.DataProtection.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.DataProtectionManagement;
public abstract class DataProtectionManagementApplicationServiceBase : ApplicationService
{
    protected DataProtectionManagementApplicationServiceBase()
    {
        LocalizationResource = typeof(DataProtectionResource);
        ObjectMapperContext = typeof(AbpDataProtectionManagementApplicationModule);
    }
}
