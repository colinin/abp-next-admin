using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Wrapper
{
    [DependsOn(typeof(AbpExceptionHandlingModule))]
    public class AbpWrapperModule: AbpModule
    {

    }
}
