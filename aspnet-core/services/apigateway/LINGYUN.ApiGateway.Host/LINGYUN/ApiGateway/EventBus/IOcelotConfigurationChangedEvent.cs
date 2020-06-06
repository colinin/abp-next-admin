using System.Threading.Tasks;

namespace LINGYUN.ApiGateway.EventBus
{
    public interface IOcelotConfigurationChangedEvent
    {
        Task OnOcelotConfigurationChanged(ApigatewayConfigChangeEventData changeCommand);
    }
}
