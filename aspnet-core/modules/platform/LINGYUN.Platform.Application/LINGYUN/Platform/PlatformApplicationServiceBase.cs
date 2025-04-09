using LINGYUN.Platform.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform;

public abstract class PlatformApplicationServiceBase : ApplicationService
{
    protected PlatformApplicationServiceBase()
    {
        LocalizationResource = typeof(PlatformResource);
        ObjectMapperContext = typeof(PlatformApplicationModule);
    }
}
