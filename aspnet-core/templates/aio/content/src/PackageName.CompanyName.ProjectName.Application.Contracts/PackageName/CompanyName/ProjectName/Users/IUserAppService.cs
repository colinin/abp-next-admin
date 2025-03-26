using PackageName.CompanyName.ProjectName.Users.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PackageName.CompanyName.ProjectName.Users
{
    /// <summary>
    /// 用户应用服务接口
    /// </summary>
    public interface IUserAppService : 
        IApplicationService
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        Task<UserDto> CreateAsync(CreateUpdateUserDto input);

        /// <summary>
        /// 更新用户
        /// </summary>
        Task<UserDto> UpdateAsync(Guid id, CreateUpdateUserDto input);

        /// <summary>
        /// 删除用户
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 获取用户
        /// </summary>
        Task<UserDto> GetAsync(Guid id);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        Task<PagedResultDto<UserItemDto>> GetListAsync(UserPagedAndSortedResultRequestDto input);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword);

        /// <summary>
        /// 重置用户密码（管理员操作）
        /// </summary>
        Task ResetPasswordAsync(Guid id, string newPassword);

        /// <summary>
        /// 启用或禁用用户
        /// </summary>
        Task SetUserActiveStatusAsync(Guid id, bool isActive);
    }
}