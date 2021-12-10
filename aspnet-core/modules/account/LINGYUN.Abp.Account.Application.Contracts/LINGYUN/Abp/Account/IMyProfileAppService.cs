using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Account
{
    public interface IMyProfileAppService : IApplicationService
    {
        /// <summary>
        /// 获取二次认证状态
        /// </summary>
        /// <returns></returns>
        Task<TwoFactorEnabledDto> GetTwoFactorEnabledAsync();
        /// <summary>
        /// 改变二次认证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeTwoFactorEnabledAsync(TwoFactorEnabledDto input);
        /// <summary>
        /// 发送改变手机号验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendChangePhoneNumberCodeAsync(SendChangePhoneNumberCodeInput input);
        /// <summary>
        /// 改变手机绑定
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// 需二次认证,主要是为了无法用到重定向页面修改相关信息的地方（点名微信小程序）
        /// </remarks>
        Task ChangePhoneNumberAsync(ChangePhoneNumberInput input);
    }
}
