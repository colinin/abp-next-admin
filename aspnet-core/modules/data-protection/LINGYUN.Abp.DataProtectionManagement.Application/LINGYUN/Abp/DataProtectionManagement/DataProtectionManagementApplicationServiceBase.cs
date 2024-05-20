using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.DataProtectionManagement;
public abstract class DataProtectionManagementApplicationServiceBase : ApplicationService
{
    protected DataProtectionManagementApplicationServiceBase()
    {
        LocalizationResource = typeof(AbpDataProtectionManagementApplicationModule);
        ObjectMapperContext = typeof(AbpDataProtectionManagementApplicationModule);
    }
}
