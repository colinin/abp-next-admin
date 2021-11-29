using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenApi
{
    public interface IAppKeyStore
    {
        Task<AppDescriptor> FindAsync(string appKey);
    }
}
