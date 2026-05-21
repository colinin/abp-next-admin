using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Calendar;

[DependsOn(typeof(AbpTimingModule))]
public class AbpCalendarModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpCalendarOptions>(context.Configuration.GetSection("Calendar"));
    }
}
