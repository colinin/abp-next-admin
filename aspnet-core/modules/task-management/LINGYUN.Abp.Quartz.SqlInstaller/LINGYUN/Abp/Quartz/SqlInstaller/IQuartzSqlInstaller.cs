using System.Threading.Tasks;

namespace LINGYUN.Abp.Quartz.SqlInstaller;

public interface IQuartzSqlInstaller
{
    Task InstallAsync();
}
