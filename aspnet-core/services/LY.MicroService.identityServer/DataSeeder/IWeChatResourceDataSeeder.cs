using System.Threading.Tasks;

namespace LY.MicroService.IdentityServer
{
    public interface IWeChatResourceDataSeeder
    {
        Task CreateStandardResourcesAsync();
    }
}
