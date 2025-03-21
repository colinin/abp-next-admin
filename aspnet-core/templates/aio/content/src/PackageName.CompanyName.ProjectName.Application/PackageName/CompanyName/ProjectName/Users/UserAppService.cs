using Microsoft.AspNetCore.Authorization;
using PackageName.CompanyName.ProjectName.Permissions;
using PackageName.CompanyName.ProjectName.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace PackageName.CompanyName.ProjectName.Users
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;
        private readonly IdentityUserManager _identityUserManager;

        public UserAppService(
            IUserRepository userRepository,
            IUserManager userManager,
            IdentityUserManager identityUserManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _identityUserManager = identityUserManager;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        [Authorize(ProjectNamePermissions.User.Create)]
        public async Task<UserDto> CreateAsync(CreateUpdateUserDto input)
        {
            // 参数检查和验证逻辑可以在这里添加
            if (string.IsNullOrEmpty(input.NickName))
            {
                throw new UserFriendlyException("昵称不能为空");
            }
            if (string.IsNullOrEmpty(input.Password))
            {
                throw new UserFriendlyException("密码不能为空");
            }

            // 使用UserManager创建用户
            var user = await _userManager.CreateAsync(
                input.NickName,
                input.Password,
                input.ContactInfo,
                input.Position,
                input.IsActive);

            // 返回DTO对象
            return await MapToUserDtoAsync(user);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        [Authorize(ProjectNamePermissions.User.Update)]
        public async Task<UserDto> UpdateAsync(Guid id, CreateUpdateUserDto input)
        {
            // 使用UserManager更新用户基本信息
            var user = await _userManager.UpdateAsync(
                id,
                input.NickName,
                input.Password,
                input.ContactInfo,
                input.Position,
                input.IsActive);

            // 确保更新后的用户状态与DTO中的一致
            if (user.IdentityUser != null)
            {
                bool currentIsActive = user.IdentityUser.LockoutEnd == null || user.IdentityUser.LockoutEnd < DateTimeOffset.Now;
                if (currentIsActive != input.IsActive)
                {
                    await _userManager.SetUserActiveStatusAsync(id, input.IsActive);
                    user = await _userManager.GetAsync(id);
                }
            }

            // 返回DTO对象
            return await MapToUserDtoAsync(user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        [Authorize(ProjectNamePermissions.User.Delete)]
        public Task DeleteAsync(Guid id)
        {
            return _userManager.DeleteAsync(id);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        [Authorize(ProjectNamePermissions.User.Default)]
        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userManager.GetAsync(id);
            return await MapToUserDtoAsync(user);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        [Authorize(ProjectNamePermissions.User.Default)]
        public async Task<PagedResultDto<UserItemDto>> GetListAsync(UserPagedAndSortedResultRequestDto input)
        {
            // 创建查询
            var query = await CreateFilteredQueryAsync(input);

            // 获取总记录数
            var totalCount = await AsyncExecuter.CountAsync(query);

            // 获取已排序和分页的查询结果
            var users = await AsyncExecuter.ToListAsync(
                query.OrderBy(input.Sorting ?? nameof(User.NickName))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount));

            // 转换为DTO并返回
            var userDtos = new List<UserItemDto>();
            foreach (var user in users)
            {
                var userDto = ObjectMapper.Map<User, UserItemDto>(user);

                // 填充角色信息
                if (user.IdentityUser != null)
                {
                    var roles = await _identityUserManager.GetRolesAsync(user.IdentityUser);
                    userDto.RoleNames = string.Join("、", roles);
                    userDto.IsActive = user.IdentityUser.LockoutEnd == null || user.IdentityUser.LockoutEnd < DateTimeOffset.Now;
                }

                userDtos.Add(userDto);
            }

            return new PagedResultDto<UserItemDto>(totalCount, userDtos);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        [Authorize]
        public async Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword)
        {
            await _userManager.ChangePasswordAsync(id, currentPassword, newPassword);
        }

        /// <summary>
        /// 重置用户密码（管理员操作）
        /// </summary>
        [Authorize(ProjectNamePermissions.User.Update)]
        public async Task ResetPasswordAsync(Guid id, string newPassword)
        {
            await _userManager.ResetPasswordAsync(id, newPassword);
        }

        /// <summary>
        /// 启用或禁用用户
        /// </summary>
        [Authorize(ProjectNamePermissions.User.Update)]
        public async Task SetUserActiveStatusAsync(Guid id, bool isActive)
        {
            await _userManager.SetUserActiveStatusAsync(id, isActive);
        }

        /// <summary>
        /// 创建基础查询，应用过滤条件
        /// </summary>
        protected async virtual Task<IQueryable<User>> CreateFilteredQueryAsync(
            UserPagedAndSortedResultRequestDto input)
        {
            // 获取基础查询，并加载相关实体
            var query = await _userRepository.WithDetailsAsync(
                x => x.IdentityUser);

            // 应用过滤条件
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(input.NickName),
                    x => x.NickName.Contains(input.NickName));
        }

        /// <summary>
        /// 将User实体映射为UserDto，并填充权限信息
        /// </summary>
        private async Task<UserDto> MapToUserDtoAsync(User user)
        {
            var userDto = ObjectMapper.Map<User, UserDto>(user);

            // 设置用户状态和角色信息
            if (user.IdentityUser != null)
            {
                var roles = await _identityUserManager.GetRolesAsync(user.IdentityUser);
                userDto.RoleNames = string.Join("、", roles);
                userDto.IsActive = user.IdentityUser.LockoutEnd == null || user.IdentityUser.LockoutEnd < DateTimeOffset.Now;
            }

            return userDto;
        }
    }
}