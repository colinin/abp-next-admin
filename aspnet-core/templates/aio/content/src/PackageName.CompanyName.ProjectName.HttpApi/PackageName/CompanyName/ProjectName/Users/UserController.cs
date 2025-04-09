using Microsoft.AspNetCore.Mvc;
using PackageName.CompanyName.ProjectName.Users.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace PackageName.CompanyName.ProjectName.Users
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    [RemoteService]
    [Route("api/app/user")]
    public class UserController : ProjectNameControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        [HttpPost]
        public async Task<UserDto> CreateAsync(CreateUpdateUserDto input)
        {
            return await _userAppService.CreateAsync(input);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        [HttpPut("{id}")]
        public async Task<UserDto> UpdateAsync(Guid id, CreateUpdateUserDto input)
        {
            return await _userAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _userAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        [HttpGet("{id}")]
        public async Task<UserDto> GetAsync(Guid id)
        {
            return await _userAppService.GetAsync(id);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        [HttpGet]
        public async Task<PagedResultDto<UserItemDto>> GetListAsync(UserPagedAndSortedResultRequestDto input)
        {
            return await _userAppService.GetListAsync(input);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        [HttpPost("{id}/change-password")]
        public async Task ChangePasswordAsync(Guid id, [FromBody] ChangePasswordRequest request)
        {
            await _userAppService.ChangePasswordAsync(id, request.CurrentPassword, request.NewPassword);
        }

        /// <summary>
        /// 重置用户密码（管理员操作）
        /// </summary>
        [HttpPost("{id}/reset-password")]
        public async Task ResetPasswordAsync(Guid id, [FromBody] ResetPasswordRequest request)
        {
            await _userAppService.ResetPasswordAsync(id, request.NewPassword);
        }

        /// <summary>
        /// 启用或禁用用户
        /// </summary>
        [HttpPost("{id}/set-active")]
        public async Task SetUserActiveStatusAsync(Guid id, [FromBody] SetUserActiveRequest request)
        {
            await _userAppService.SetUserActiveStatusAsync(id, request.IsActive);
        }
    }

    /// <summary>
    /// 修改密码请求
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
    }

    /// <summary>
    /// 重置密码请求
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
    }

    /// <summary>
    /// 设置用户状态请求
    /// </summary>
    public class SetUserActiveRequest
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
