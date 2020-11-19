using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    public interface IIdentityUserRepository : Volo.Abp.Identity.IIdentityUserRepository
    {
        /// <summary>
        /// 手机号是否已被使用
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsPhoneNumberUedAsync(
            string phoneNumber,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 手机号是否已确认(绑定)
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsPhoneNumberConfirmedAsync(
            string phoneNumber,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 邮件地址是否已确认(绑定)
        /// </summary>
        /// <param name="normalizedEmail"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsNormalizedEmailConfirmedAsync(
           string normalizedEmail,
           CancellationToken cancellationToken = default);
        /// <summary>
        /// 通过手机号查询用户
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="isConfirmed">是否已确认过</param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IdentityUser> FindByPhoneNumberAsync(
            string phoneNumber,
            bool isConfirmed = true,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 通过用户主键列表获取用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<IdentityUser>> GetListByIdListAsync(
            List<Guid> userIds,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            );
        /// <summary>
        /// 获取用户所有的组织机构列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filter"></param>
        /// <param name="includeDetails"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            Guid userId, 
            string filter = null,
            bool includeDetails = false, 
            int skipCount = 1, 
            int maxResultCount = 10, 
            CancellationToken cancellationToken = default
        );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetUsersInOrganizationUnitCountAsync(
            Guid organizationUnitId,
            string filter = null,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(
            Guid organizationUnitId,
            string filter = null,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
        );

        Task<long> GetUsersInOrganizationsListCountAsync(
            List<Guid> organizationUnitIds,
            string filter = null,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
            List<Guid> organizationUnitIds,
            string filter = null,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
        );

        Task<long> GetUsersInOrganizationUnitWithChildrenCountAsync(
            string code,
            string filter = null,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityUser>> GetUsersInOrganizationUnitWithChildrenAsync(
            string code,
            string filter = null,
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default
        );
    }
}
