using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

[Dependency(TryRegister = true)]
public class DefaultUserPictureProvider : IUserPictureProvider, ISingletonDependency
{
    public Task SetPictureAsync(IdentityUser user, Stream stream, string fileName = null)
    {
        return Task.CompletedTask;
    }

    public Task<Stream> GetPictureAsync(string userId)
    {
        return Task.FromResult(Stream.Null);
    }
}
