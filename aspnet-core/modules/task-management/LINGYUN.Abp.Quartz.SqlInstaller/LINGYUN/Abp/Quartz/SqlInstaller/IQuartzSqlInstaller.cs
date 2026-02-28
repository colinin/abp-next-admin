using System.Threading.Tasks;

namespace LINGYUN.Abp.Quartz.SqlInstaller;

public interface IQuartzSqlInstaller
{
    bool CanInstall(string driverDelegateType);

    Task InstallAsync();
}
