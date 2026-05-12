using LINGYUN.Abp.BlobManagement.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.BlobManagement;

public abstract class BlobManagementApplicationService : ApplicationService
{
    protected BlobManagementApplicationService()
    {
        LocalizationResource = typeof(BlobManagementResource);
        ObjectMapperContext = typeof(AbpBlobManagementApplicationModule);
    }
}
