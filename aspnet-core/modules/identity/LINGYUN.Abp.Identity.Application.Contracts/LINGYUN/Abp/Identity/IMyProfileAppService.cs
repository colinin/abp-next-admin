using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Identity
{
    public interface IMyProfileAppService : IApplicationService
    {
        /// <summary>
        /// 变更双因素验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeTwoFactorEnabledAsync(IdentityUserTwoFactorEnabledDto input);
    }
}
