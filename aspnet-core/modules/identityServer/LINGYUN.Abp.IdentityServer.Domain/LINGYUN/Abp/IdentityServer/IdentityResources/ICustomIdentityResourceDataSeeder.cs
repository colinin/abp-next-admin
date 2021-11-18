using System.Threading.Tasks;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    public interface ICustomIdentityResourceDataSeeder
    {
        Task CreateCustomResourcesAsync();
    }
}
