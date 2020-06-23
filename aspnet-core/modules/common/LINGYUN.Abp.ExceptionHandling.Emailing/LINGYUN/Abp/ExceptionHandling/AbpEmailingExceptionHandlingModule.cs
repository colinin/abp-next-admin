using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ExceptionHandling.Emailing
{
    [DependsOn(typeof(AbpExceptionHandlingModule))]
    public class AbpEmailingExceptionHandlingModule : AbpModule
    {

    }
}
