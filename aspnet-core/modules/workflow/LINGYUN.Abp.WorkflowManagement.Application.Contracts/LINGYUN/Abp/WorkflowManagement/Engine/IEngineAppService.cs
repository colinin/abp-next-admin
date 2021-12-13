using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WorkflowManagement.Engine
{
    public interface IEngineAppService : IApplicationService
    {
        Task InitializeAsync();

        Task RegisterAsync();
    }
}
