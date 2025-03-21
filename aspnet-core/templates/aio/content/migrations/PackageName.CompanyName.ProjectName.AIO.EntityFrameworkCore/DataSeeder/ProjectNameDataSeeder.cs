using Microsoft.Extensions.Logging;
using PackageName.CompanyName.ProjectName.Users;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DataSeeder
{
    public class ProjectNameDataSeeder : IProjectNameDataSeeder, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<ProjectNameDataSeeder> _logger;
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IdentityRoleManager _identityRoleManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectNameDataSeeder(
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator,
            ILogger<ProjectNameDataSeeder> logger,
            IRepository<User, Guid> userRepository,
            IdentityUserManager identityUserManager,
            IdentityRoleManager identityRoleManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _logger = logger;
            _userRepository = userRepository;
            _identityUserManager = identityUserManager;
            _identityRoleManager = identityRoleManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="context">数据种子上下文</param>
        /// <returns>任务</returns>
        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context.TenantId))
            {
                _logger.LogInformation("开始初始化数据...");

                // 初始化角色
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
                {
                    await SeedRolesAsync();
                    await uow.CompleteAsync();
                }

                // 初始化用户数据
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
                {
                    await SeedUsersAsync();
                    await uow.CompleteAsync();
                }

                _logger.LogInformation("数据初始化完成");
            }
        }

        /// <summary>
        /// 初始化角色数据
        /// </summary>
        private async Task SeedRolesAsync()
        {
            // 超级管理员
            await CreateRoleIfNotExistsAsync(
                "超级管理员",
                "系统超级管理员，拥有所有权限");
        }

        /// <summary>
        /// 创建角色（如果不存在）
        /// </summary>
        private async Task CreateRoleIfNotExistsAsync(string roleName, string description)
        {
            if (await _identityRoleManager.FindByNameAsync(roleName) == null)
            {
                var role = new IdentityRole(
                    _guidGenerator.Create(),
                    roleName,
                    _currentTenant.Id)
                {
                    IsStatic = true,
                    IsPublic = true
                };

                await _identityRoleManager.CreateAsync(role);

                _logger.LogInformation($"创建角色：{roleName}");
            }
        }

        /// <summary>
        /// 初始化用户数据
        /// </summary>
        private async Task SeedUsersAsync()
        {

            // 查找超级管理员角色
            var superAdminRole = await _identityRoleManager.FindByNameAsync("超级管理员");
            if (superAdminRole == null)
            {
                _logger.LogError("未找到超级管理员角色，无法为用户分配角色");
                return;
            }

            // 创建用户数据（使用固定用户名避免生成问题）
            await CreateUserIfNotExistsAsync("user1", "user1", "超级管理员");
            await CreateUserIfNotExistsAsync("user2", "user2", "超级管理员");
            await CreateUserIfNotExistsAsync("user3", "user3",  "超级管理员");
            await CreateUserIfNotExistsAsync("user4", "user4", "超级管理员");
        }

        /// <summary>
        /// 创建用户（如果不存在）
        /// </summary>
        private async Task CreateUserIfNotExistsAsync(
            string name,
            string userName,
            string roles)
        {
            // 检查用户是否已存在
            var existingUser = await _userRepository.FindAsync(u => u.NickName == name);
            if (existingUser != null)
            {
                _logger.LogInformation($"用户[{name}]已存在，跳过创建");
                return;
            }

            var identityUser = await _identityUserManager.FindByNameAsync(userName);
            if (identityUser == null)
            {
                // 创建Identity用户
                identityUser = new IdentityUser(
                    _guidGenerator.Create(),
                    userName,
                    $"{userName}@example.com",
                    _currentTenant.Id)
                {
                    Name = name,
                    Surname = ""
                };

                // 设置默认密码 123456
                var identityResult = await _identityUserManager.CreateAsync(identityUser, "123456");
                if (!identityResult.Succeeded)
                {
                    _logger.LogError($"创建Identity用户[{name}]失败: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}");
                    return;
                }

                // 分配角色
                if (!string.IsNullOrWhiteSpace(roles))
                {
                    var roleNames = roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var roleName in roleNames)
                    {
                        var trimmedRoleName = roleName.Trim();
                        var role = await _identityRoleManager.FindByNameAsync(trimmedRoleName);
                        if (role != null)
                        {
                            var roleResult = await _identityUserManager.AddToRoleAsync(identityUser, trimmedRoleName);
                            if (!roleResult.Succeeded)
                            {
                                _logger.LogWarning($"为用户[{name}]分配角色[{trimmedRoleName}]失败: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                            }
                        }
                        else
                        {
                            _logger.LogWarning($"角色[{trimmedRoleName}]不存在，无法为用户[{name}]分配");
                        }
                    }
                }

                // 创建系统用户
                var user = new User(
                    _guidGenerator.Create(),
                    name,
                    identityUser.Id);

                // 保存用户
                await _userRepository.InsertAsync(user);

                _logger.LogInformation($"创建用户：{name}，用户名：{userName}");
            }
            else
            {
                _logger.LogInformation($"Identity用户[{userName}]已存在，检查是否需要创建业务用户");

                // 检查是否需要创建业务用户
                var businessUser = await _userRepository.FindAsync(u => u.IdentityUserId == identityUser.Id);
                if (businessUser == null)
                {
                    // 创建系统用户
                    var user = new User(
                        _guidGenerator.Create(),
                        name,
                        identityUser.Id);

                    // 保存用户
                    await _userRepository.InsertAsync(user);

                    _logger.LogInformation($"为已存在的Identity用户创建业务用户：{name}");
                }
            }
        }
    }
}
