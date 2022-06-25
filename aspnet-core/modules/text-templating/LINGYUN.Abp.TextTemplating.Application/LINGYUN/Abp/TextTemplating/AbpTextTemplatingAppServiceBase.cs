using LINGYUN.Abp.TextTemplating.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.TextTemplating;

public abstract class AbpTextTemplatingAppServiceBase : ApplicationService
{
    protected AbpTextTemplatingAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpTextTemplatingApplicationModule);
        LocalizationResource = typeof(AbpTextTemplatingResource);
    }
}
