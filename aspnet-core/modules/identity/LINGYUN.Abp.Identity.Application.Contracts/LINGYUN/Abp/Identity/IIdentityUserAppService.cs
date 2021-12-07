using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    public interface IIdentityUserAppService : IApplicationService
    {

        #region OrganizationUnit

        Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id);

        Task SetOrganizationUnitsAsync(Guid id, IdentityUserOrganizationUnitUpdateDto input);

        Task RemoveOrganizationUnitsAsync(Guid id, Guid ouId);

        #endregion

        #region ClaimType

        Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id);

        Task AddClaimAsync(Guid id, IdentityUserClaimCreateDto input);

        Task UpdateClaimAsync(Guid id, IdentityUserClaimUpdateDto input);

        Task DeleteClaimAsync(Guid id, IdentityUserClaimDeleteDto input);

        #endregion

        /// <summary>
        /// 变更用户双因素验证选项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeTwoFactorEnabledAsync(Guid id, TwoFactorEnabledDto input);
        /// <summary>
        /// 变更用户密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// TODO: 移除api，改为重置用户密码
        // Task ChangePasswordAsync(Guid id, ChangePasswordInput input);
        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="id"></param>
        /// <param name="seconds">锁定时长</param>
        /// <returns></returns>
        Task LockAsync(Guid id, int seconds);
        /// <summary>
        /// 解除锁定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UnLockAsync(Guid id);
    }
}
