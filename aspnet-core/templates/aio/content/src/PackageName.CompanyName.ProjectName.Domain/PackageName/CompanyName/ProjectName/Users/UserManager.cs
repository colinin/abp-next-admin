using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace PackageName.CompanyName.ProjectName.Users
{
    /// <summary>
    /// 用户管理服务，用于处理用户的CRUD操作
    /// </summary>
    public class UserManager : DomainService, IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IdentityUserManager _identityUserManager;
        private readonly ILogger<UserManager> _logger;

        public UserManager(
            IUserRepository userRepository,
            IdentityUserManager identityUserManager,
            ILogger<UserManager> logger)
        {
            _userRepository = userRepository;
            _identityUserManager = identityUserManager;
            _logger = logger;
        }

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="nickName">用户昵称</param>
        /// <param name="password">用户密码</param>
        /// <param name="contactInfo">联系方式</param>
        /// <param name="position">职位</param>
        /// <param name="isActive">是否启用</param>
        /// <returns>创建的用户实体</returns>
        public async Task<User> CreateAsync(
            string nickName,
            string password,
            string contactInfo = null,
            string position = null,
            bool isActive = true)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(nickName))
            {
                throw new UserFriendlyException("用户名不能为空");
            }

            // 密码校验
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                throw new UserFriendlyException("密码不能为空且长度不能少于6位");
            }

            // 检查用户昵称是否已存在
            var existingUser = await _userRepository.FindAsync(u => u.NickName == nickName);
            if (existingUser != null)
            {
                throw new UserFriendlyException($"昵称为 '{nickName}' 的用户已存在");
            }

            // 创建Identity用户
            var identityUser = new IdentityUser(GuidGenerator.Create(), nickName, $"{nickName}@inspection.com");
            var identityResult = await _identityUserManager.CreateAsync(identityUser, password);
            if (!identityResult.Succeeded)
            {
                throw new UserFriendlyException("创建用户失败: " +
                                                string.Join(", ", identityResult.Errors.Select(x => x.Description)));
            }

            // 设置用户状态
            if (!isActive)
            {
                var lockoutResult = await _identityUserManager.SetLockoutEndDateAsync(identityUser, DateTimeOffset.MaxValue);
                if (!lockoutResult.Succeeded)
                {
                    throw new UserFriendlyException("设置用户状态失败: " +
                        string.Join(", ", lockoutResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                // 确保用户处于活动状态
                await _identityUserManager.SetLockoutEnabledAsync(identityUser, false);
                var lockoutResult = await _identityUserManager.SetLockoutEndDateAsync(identityUser, null);
                if (!lockoutResult.Succeeded)
                {
                    throw new UserFriendlyException("设置用户状态失败: " +
                        string.Join(", ", lockoutResult.Errors.Select(e => e.Description)));
                }
            }

            // 创建业务用户
            var user = new User(
                GuidGenerator.Create(),
                nickName,
                identityUser.Id,
                contactInfo,
                position
            );

            // 保存用户
            await _userRepository.InsertAsync(user, true);

            _logger.LogInformation($"创建了新用户：{nickName}，ID：{user.Id}");

            return user;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="nickName">用户昵称</param>
        /// <param name="password">用户密码（可选，如不修改则传入null）</param>
        /// <param name="contactInfo">联系方式</param>
        /// <param name="position">职位</param>
        /// <param name="isActive">是否启用</param>
        /// <returns>更新后的用户实体</returns>
        public async Task<User> UpdateAsync(
            Guid id,
            string nickName,
            string password,
            string contactInfo = null,
            string position = null,
            bool isActive = true)
        {
            // 最大重试次数
            const int maxRetries = 3;
            int retryCount = 0;

            while (true)
            {
                try
                {
                    // 每次尝试时重新获取最新的用户数据
                    var user = await _userRepository.GetAsync(id, false);
                    if (user == null)
                    {
                        throw new UserFriendlyException("用户不存在");
                    }
                    
                    // 检查用户昵称是否已被其他用户使用
                    var existingUser = await _userRepository.FindAsync(u => u.NickName == nickName && u.Id != id);
                    if (existingUser != null)
                    {
                        throw new UserFriendlyException($"昵称为 '{nickName}' 的用户已存在");
                    }

                    // 更新Identity用户
                    var identityUser = await _identityUserManager.FindByIdAsync(user.IdentityUserId.ToString());
                    if (identityUser == null)
                    {
                        throw new UserFriendlyException("Identity用户不存在");
                    }

                    // 更新用户名
                    var usernameResult = await _identityUserManager.SetUserNameAsync(identityUser, nickName);
                    if (!usernameResult.Succeeded)
                    {
                        throw new UserFriendlyException("更新用户名失败: " +
                            string.Join(", ", usernameResult.Errors.Select(e => e.Description)));
                    }

                    // 更新电子邮件
                    var emailResult = await _identityUserManager.SetEmailAsync(identityUser, $"{nickName}@inspection.com");
                    if (!emailResult.Succeeded)
                    {
                        throw new UserFriendlyException("更新电子邮件失败: " +
                            string.Join(", ", emailResult.Errors.Select(e => e.Description)));
                    }

                    // 如果提供了新密码，则更新密码
                    if (!string.IsNullOrEmpty(password))
                    {
                        // 移除当前密码
                        await _identityUserManager.RemovePasswordAsync(identityUser);
                        // 设置新密码
                        var passwordResult = await _identityUserManager.AddPasswordAsync(identityUser, password);
                        if (!passwordResult.Succeeded)
                        {
                            throw new UserFriendlyException("更新密码失败: " +
                                string.Join(", ", passwordResult.Errors.Select(e => e.Description)));
                        }
                    }

                    // 设置用户状态
                    await SetUserActiveStatusAsync(id, isActive);

                    // 更新用户信息
                    user.NickName = nickName;
                    user.ContactInfo = contactInfo;
                    user.Position = position;

                    // 保存更新
                    await _userRepository.UpdateAsync(user, true);

                    _logger.LogInformation($"更新了用户信息：{nickName}，ID：{user.Id}");

                    return user;
                }
                catch (Volo.Abp.Data.AbpDbConcurrencyException ex)
                {
                    // 增加重试计数
                    retryCount++;

                    // 如果达到最大重试次数，则抛出用户友好的异常
                    if (retryCount >= maxRetries)
                    {
                        throw new UserFriendlyException(
                            "更新用户信息失败：数据已被其他用户修改。请刷新页面后重试。",
                            "409", ex.Message,
                            ex);
                    }

                    // 短暂延迟后重试
                    await Task.Delay(100 * retryCount); // 逐步增加延迟时间

                    // 记录重试信息
                    _logger.LogWarning($"检测到用户[{id}]更新时发生并发冲突，正在进行第{retryCount}次重试...");
                }
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>操作任务</returns>
        public async Task DeleteAsync(Guid id)
        {
            // 最大重试次数
            const int maxRetries = 3;
            int retryCount = 0;

            while (true)
            {
                try
                {
                    // 每次尝试时重新获取最新的用户数据
                    var user = await _userRepository.GetAsync(id);
                    if (user == null)
                    {
                        _logger.LogWarning($"尝试删除不存在的用户，ID：{id}");
                        return; // 如果用户不存在，就不再继续处理
                    }

                    // 删除Identity用户
                    var identityUser = await _identityUserManager.FindByIdAsync(user.IdentityUserId.ToString());
                    if (identityUser != null)
                    {
                        var result = await _identityUserManager.DeleteAsync(identityUser);
                        if (!result.Succeeded)
                        {
                            throw new UserFriendlyException("删除Identity用户失败: " +
                                string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }

                    // 删除用户
                    await _userRepository.DeleteAsync(user);

                    _logger.LogInformation($"删除了用户，ID：{id}");
                    return;
                }
                catch (Volo.Abp.Data.AbpDbConcurrencyException ex)
                {
                    // 增加重试计数
                    retryCount++;

                    // 如果达到最大重试次数，则抛出用户友好的异常
                    if (retryCount >= maxRetries)
                    {
                        throw new UserFriendlyException(
                            "删除用户失败：数据已被其他用户修改。请刷新页面后重试。",
                            "409", ex.Message,
                            ex);
                    }

                    // 短暂延迟后重试
                    await Task.Delay(100 * retryCount); // 逐步增加延迟时间

                    // 记录重试信息
                    _logger.LogWarning($"检测到用户[{id}]删除时发生并发冲突，正在进行第{retryCount}次重试...");
                }
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="currentPassword">当前密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>操作结果</returns>
        public async Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
            {
                throw new UserFriendlyException("新密码不能为空且长度不能少于6位");
            }

            // 获取用户
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }

            // 获取Identity用户
            var identityUser = await _identityUserManager.FindByIdAsync(user.IdentityUserId.ToString());
            if (identityUser == null)
            {
                throw new UserFriendlyException("Identity用户不存在");
            }

            // 修改密码
            var result = await _identityUserManager.ChangePasswordAsync(identityUser, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new UserFriendlyException("修改密码失败: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            _logger.LogInformation($"用户[{user.NickName}]成功修改了密码");
        }

        /// <summary>
        /// 重置用户密码（管理员操作）
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>操作结果</returns>
        public async Task ResetPasswordAsync(Guid id, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
            {
                throw new UserFriendlyException("新密码不能为空且长度不能少于6位");
            }

            // 获取用户
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }

            // 获取Identity用户
            var identityUser = await _identityUserManager.FindByIdAsync(user.IdentityUserId.ToString());
            if (identityUser == null)
            {
                throw new UserFriendlyException("Identity用户不存在");
            }

            // 生成重置令牌
            var token = await _identityUserManager.GeneratePasswordResetTokenAsync(identityUser);

            // 重置密码
            var result = await _identityUserManager.ResetPasswordAsync(identityUser, token, newPassword);
            if (!result.Succeeded)
            {
                throw new UserFriendlyException("重置密码失败: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            _logger.LogInformation($"管理员重置了用户[{user.NickName}]的密码");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户实体</returns>
        public async Task<User> GetAsync(Guid id)
        {
            return await _userRepository.GetAsync(id);
        }

        /// <summary>
        /// 根据Identity用户ID获取用户
        /// </summary>
        /// <param name="identityUserId">Identity用户ID</param>
        /// <returns>用户实体，如果不存在则返回null</returns>
        public async Task<User> FindByIdentityUserIdAsync(Guid identityUserId)
        {
            return await _userRepository.FindAsync(u => u.IdentityUserId == identityUserId);
        }

        /// <summary>
        /// 根据用户昵称查找用户
        /// </summary>
        /// <param name="nickName">用户昵称</param>
        /// <returns>用户实体，如果不存在则返回null</returns>
        public async Task<User> FindByNickNameAsync(string nickName)
        {
            return await _userRepository.FindAsync(u => u.NickName == nickName);
        }

        /// <summary>
        /// 禁用或启用用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="isActive">是否启用</param>
        /// <returns>操作任务</returns>
        public async Task SetUserActiveStatusAsync(Guid id, bool isActive)
        {
            // 获取用户
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }

            // 获取Identity用户
            var identityUser = await _identityUserManager.FindByIdAsync(user.IdentityUserId.ToString());
            if (identityUser == null)
            {
                throw new UserFriendlyException("Identity用户不存在");
            }

            // 设置用户状态
            if (isActive)
            {
                // 启用用户
                var result = await _identityUserManager.SetLockoutEndDateAsync(identityUser, null);
                if (!result.Succeeded)
                {
                    throw new UserFriendlyException($"启用用户失败: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                // 禁用用户（设置永久锁定）
                var result = await _identityUserManager.SetLockoutEndDateAsync(identityUser, DateTimeOffset.MaxValue);
                if (!result.Succeeded)
                {
                    throw new UserFriendlyException($"禁用用户失败: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            _logger.LogInformation($"已{(isActive ? "启用" : "禁用")}用户[{user.NickName}]");
        }
    }
}