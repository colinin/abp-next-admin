using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WorkflowManagement.Events
{
    public interface IEventAppService : IApplicationService
    {
        Task PublishAsync(EventPublishInput input);
    }
}
