using System.Threading.Tasks;

namespace LINGYUN.Abp.RealTime.Client
{
    public interface IClient
    {
        Task OnConnectedAsync(IOnlineClient client);
        Task OnDisconnectedAsync(IOnlineClient client);
    }
}
