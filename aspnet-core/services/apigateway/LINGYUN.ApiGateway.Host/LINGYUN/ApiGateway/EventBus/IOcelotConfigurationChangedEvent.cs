using System.Threading.Tasks;

namespace LINGYUN.ApiGateway.EventBus
{
    public interface IOcelotConfigurationChangedEvent
    {
        Task OnOcelotConfigurationChanged(ApigatewayConfigChangeCommand changeCommand);
    }
}
