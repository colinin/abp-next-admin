using System.Threading.Tasks;

namespace LINGYUN.Abp.Cli.Commands
{
    public interface ICreateProjectService
    {
        Task CreateAsync(ProjectCreateArgs createArgs);
    }
}
