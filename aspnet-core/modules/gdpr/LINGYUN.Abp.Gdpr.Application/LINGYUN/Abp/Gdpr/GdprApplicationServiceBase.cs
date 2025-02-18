using LINGYUN.Abp.Gdpr.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Gdpr;
public abstract class GdprApplicationServiceBase : ApplicationService
{
    protected GdprApplicationServiceBase()
    {
        LocalizationResource = typeof(GdprResource);
        ObjectMapperContext = typeof(AbpGdprApplicationModule);
    }
}
