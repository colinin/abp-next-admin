using System.Threading.Tasks;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.Migrations;

public interface IElsaDataBaseInstaller
{
    Task InstallAsync();
}
