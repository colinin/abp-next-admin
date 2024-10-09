using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Demo;
public abstract class DemoApplicationServiceBase : ApplicationService
{
    protected DemoApplicationServiceBase()
    {
        LocalizationResource = typeof(AbpDemoApplicationModule);
        ObjectMapperContext = typeof(AbpDemoApplicationModule);
    }
}
