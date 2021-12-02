using Hangfire;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Hangfire.Dashboard
{
    public class AbpHangfireDashboardOptionsProvider : ITransientDependency
    {
        public virtual DashboardOptions Get()
        {
            return new DashboardOptions();
        }
    }
}
