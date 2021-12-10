using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Account
{
    public interface IMyClaimAppService : IApplicationService
    {
        Task ChangeAvatarAsync(ChangeAvatarInput input);
    }
}
