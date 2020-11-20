using System.Threading.Tasks;

namespace LINGYUN.Abp.IdentityServer
{
    public interface IWeChatResourceDataSeeder
    {
        Task CreateStandardResourcesAsync();
    }
}
