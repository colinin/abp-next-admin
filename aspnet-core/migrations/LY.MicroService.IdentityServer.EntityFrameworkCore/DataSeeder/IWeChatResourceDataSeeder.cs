using System.Threading.Tasks;

namespace LY.MicroService.IdentityServer.EntityFrameworkCore.DataSeeder;

public interface IWeChatResourceDataSeeder
{
    Task CreateStandardResourcesAsync();
}
