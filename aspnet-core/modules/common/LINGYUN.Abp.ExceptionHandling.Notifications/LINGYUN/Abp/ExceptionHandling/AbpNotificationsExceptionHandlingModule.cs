using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ExceptionHandling
{
    [DependsOn(typeof(AbpExceptionHandlingModule))]
    public class AbpNotificationsExceptionHandlingModule : AbpModule
    {
    }
}
