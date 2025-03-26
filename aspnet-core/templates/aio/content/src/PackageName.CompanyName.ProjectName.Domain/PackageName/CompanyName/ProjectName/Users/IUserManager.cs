using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace PackageName.CompanyName.ProjectName.Users
{
    /// <summary>
    /// 用户管理服务接口
    /// </summary>
    public interface IUserManager : IDomainService
    {
        /// <summary>
        /// 创建新用户
        /// </summary>
        Task<User> CreateAsync(
            string nickName,
            string password,
            string contactInfo = null,
            string position = null,
            bool isActive = true
            );

        /// <summary>
        /// 更新用户信息
        /// </summary>
        Task<User> UpdateAsync(
            Guid id,
            string nickName,
            string password,
            string contactInfo = null,
            string position = null,
            bool isActive = true);

        /// <summary>
        /// 删除用户
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword);

        /// <summary>
        /// 重置用户密码
        /// </summary>
        Task ResetPasswordAsync(Guid id, string newPassword);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        Task<User> GetAsync(Guid id);

        /// <summary>
        /// 根据Identity用户ID获取用户
        /// </summary>
        Task<User> FindByIdentityUserIdAsync(Guid identityUserId);

        /// <summary>
        /// 根据用户昵称查找用户
        /// </summary>
        Task<User> FindByNickNameAsync(string nickName);

        /// <summary>
        /// 禁用或启用用户
        /// </summary>
        Task SetUserActiveStatusAsync(Guid id, bool isActive);
    }
}