using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;

public interface IUserPictureProvider
{
    Task SetPictureAsync(IdentityUser user, Stream stream, string fileName = null);

    Task<Stream> GetPictureAsync(string userId);
}
