using Microsoft.Extensions.Logging;
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
    public class ProjectNameDataSeederDataSeeder : IProjectNameDataSeeder, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<ProjectNameDataSeeder> _logger;
        private readonly IRepository<Org, Guid> _orgRepository;
        private readonly IRepository<WorkUnit, Guid> _workUnitRepository;
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
            IRepository<Org, Guid> orgRepository,
            IRepository<WorkUnit, Guid> workUnitRepository,
            IRepository<User, Guid> userRepository,
            IdentityUserManager identityUserManager,
            IdentityRoleManager identityRoleManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _logger = logger;
            _orgRepository = orgRepository;
            _workUnitRepository = workUnitRepository;
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
                _logger.LogInformation("开始初始化巡检数据...");

                // 初始化角色
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
                {
                    await SeedRolesAsync();
                    await uow.CompleteAsync();
                }

                // 初始化单位数据
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
                {
                    await SeedWorkUnitsAsync();
                    await uow.CompleteAsync();
                }

                // 初始化组织数据
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
                {
                    await SeedOrgsAsync();
                    await uow.CompleteAsync();
                }

                // 初始化用户数据
                using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
                {
                    await SeedUsersAsync();
                    await uow.CompleteAsync();
                }

                _logger.LogInformation("巡检数据初始化完成");
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

            // 区委办公室-督查室管理员
            await CreateRoleIfNotExistsAsync(
                "区委办公室-督查室管理员",
                "区委办公室督查室管理员");

            // 责任领导秘书
            await CreateRoleIfNotExistsAsync(
                "责任领导秘书",
                "负责协助责任领导进行工作");

            // 责任单位管理员
            await CreateRoleIfNotExistsAsync(
                "责任单位管理员",
                "负责管理责任单位的信息");

            // 责任单位落实人员
            await CreateRoleIfNotExistsAsync(
                "责任单位落实人员",
                "负责执行责任单位的任务");

            // 责任单位分管领导
            await CreateRoleIfNotExistsAsync(
                "责任单位分管领导",
                "负责管理责任单位的部分工作");

            // 责任单位党组书记
            await CreateRoleIfNotExistsAsync(
                "责任单位党组书记",
                "负责责任单位的党组工作");
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
        /// 初始化单位数据
        /// </summary>
        private async Task SeedWorkUnitsAsync()
        {
            // 创建永川区
            var yongchuanDistrict = await CreateWorkUnitIfNotExistsAsync(
                "永川区",
                "001",
                "001",
                "永川区");

            // 创建区政府
            var districtGovernment = await CreateWorkUnitIfNotExistsAsync(
                "区政府",
                "002",
                "001.002",
                "永川区政府",
                yongchuanDistrict.Id);

            // 创建区委办公室
            var districtCommitteeOffice = await CreateWorkUnitIfNotExistsAsync(
                "区委办公室",
                "003",
                "001.003",
                "永川区委办公室",
                yongchuanDistrict.Id);

            // 创建督查室
            await CreateWorkUnitIfNotExistsAsync(
                "督查室",
                "004",
                "001.003.004",
                "区委办公室督查室",
                districtCommitteeOffice.Id);

            // 创建区委扫黑办
            await CreateWorkUnitIfNotExistsAsync(
                "区委扫黑办",
                "005",
                "001.005",
                "永川区委扫黑办",
                yongchuanDistrict.Id);

            // 创建区体育局
            await CreateWorkUnitIfNotExistsAsync(
                "区体育局",
                "006",
                "001.006",
                "永川区体育局",
                yongchuanDistrict.Id);

            // 创建区统计局
            await CreateWorkUnitIfNotExistsAsync(
                "区统计局",
                "007",
                "001.007",
                "永川区统计局",
                yongchuanDistrict.Id);

            // 创建区信访办
            await CreateWorkUnitIfNotExistsAsync(
                "区信访办",
                "008",
                "001.008",
                "永川区信访办",
                yongchuanDistrict.Id);

            // 创建区医保局
            await CreateWorkUnitIfNotExistsAsync(
                "区医保局",
                "009",
                "001.009",
                "永川区医保局",
                yongchuanDistrict.Id);

            // 创建区大数据发展局
            await CreateWorkUnitIfNotExistsAsync(
                "区大数据发展局",
                "010",
                "001.010",
                "永川区大数据发展局",
                yongchuanDistrict.Id);

            // 创建区机关事务局
            await CreateWorkUnitIfNotExistsAsync(
                "区机关事务局",
                "011",
                "001.011",
                "永川区机关事务局",
                yongchuanDistrict.Id);

            // 创建区广播电视局
            await CreateWorkUnitIfNotExistsAsync(
                "区广播电视局",
                "012",
                "001.012",
                "永川区广播电视局",
                yongchuanDistrict.Id);

            // 创建区中新项目管理局
            await CreateWorkUnitIfNotExistsAsync(
                "区中新项目管理局",
                "013",
                "001.013",
                "永川区中新项目管理局",
                yongchuanDistrict.Id);

            // 创建镇街
            await CreateWorkUnitIfNotExistsAsync(
                "镇街",
                "014",
                "001.014",
                "永川区镇街",
                yongchuanDistrict.Id);
        }

        /// <summary>
        /// 创建单位（如果不存在）
        /// </summary>
        private async Task<WorkUnit> CreateWorkUnitIfNotExistsAsync(
            string name,
            string code,
            string treeCode,
            string description,
            Guid? parentId = null)
        {
            var workUnit = await _workUnitRepository.FindAsync(w => w.Name == name);

            if (workUnit == null)
            {
                workUnit = new WorkUnit(
                    _guidGenerator.Create(),
                    code,
                    treeCode,
                    name,
                    description)
                {
                    ParentId = parentId
                };

                await _workUnitRepository.InsertAsync(workUnit);

                _logger.LogInformation($"创建单位：{name}");
            }

            return workUnit;
        }

        /// <summary>
        /// 初始化组织数据
        /// </summary>
        private async Task SeedOrgsAsync()
        {
            // 创建区委办公室-督查室
            var supervisionOffice = await CreateOrgIfNotExistsAsync(
                "区委办公室-督查室",
                "001",
                "001",
                "区委办公室督查室");
        }

        /// <summary>
        /// 创建组织（如果不存在）
        /// </summary>
        private async Task<Org> CreateOrgIfNotExistsAsync(
            string name,
            string code,
            string treeCode,
            string description,
            Guid? parentId = null)
        {
            var org = await _orgRepository.FindAsync(o => o.Name == name);

            if (org == null)
            {
                org = new Org(
                    _guidGenerator.Create(),
                    code,
                    treeCode,
                    name,
                    description)
                {
                    ParentId = parentId
                };

                await _orgRepository.InsertAsync(org);

                _logger.LogInformation($"创建组织：{name}");
            }

            return org;
        }

        /// <summary>
        /// 初始化用户数据
        /// </summary>
        private async Task SeedUsersAsync()
        {
            // 获取所需单位
            var supervisionOffice = await _workUnitRepository.FindAsync(w => w.Name == "督查室");
            if (supervisionOffice == null)
            {
                _logger.LogError("未找到督查室单位，无法创建用户");
                return;
            }

            // 获取所需组织
            var supervisionOrg = await _orgRepository.FindAsync(o => o.Name == "区委办公室-督查室");
            if (supervisionOrg == null)
            {
                _logger.LogError("未找到区委办公室-督查室组织，无法创建用户");
                return;
            }

            // 查找超级管理员角色
            var superAdminRole = await _identityRoleManager.FindByNameAsync("超级管理员");
            if (superAdminRole == null)
            {
                _logger.LogError("未找到超级管理员角色，无法为用户分配角色");
                return;
            }

            // 创建用户数据（使用固定用户名避免生成问题）
            await CreateUserIfNotExistsAsync("李达康", "lidk001", GenderType.Male, supervisionOffice.Id, supervisionOrg.Id, "超级管理员");
            await CreateUserIfNotExistsAsync("高育良", "gyl002", GenderType.Male, supervisionOffice.Id, supervisionOrg.Id, "超级管理员");
            await CreateUserIfNotExistsAsync("祁同伟", "qtw003", GenderType.Male, supervisionOffice.Id, supervisionOrg.Id, "超级管理员");
            await CreateUserIfNotExistsAsync("侯亮平", "hlp004", GenderType.Male, supervisionOffice.Id, supervisionOrg.Id, "超级管理员");
            await CreateUserIfNotExistsAsync("赵瑞龙", "zrl005", GenderType.Female, supervisionOffice.Id, supervisionOrg.Id, "超级管理员");
            await CreateUserIfNotExistsAsync("宋梦平", "smp006", GenderType.Male, supervisionOffice.Id, supervisionOrg.Id, "超级管理员, 部门领导");
            await CreateUserIfNotExistsAsync("李太亮", "ltl007", GenderType.Male, supervisionOffice.Id, supervisionOrg.Id, "超级管理员, 部门领导");
            await CreateUserIfNotExistsAsync("高峰", "gf008", GenderType.Male, supervisionOffice.Id, supervisionOrg.Id, "超级管理员, 部门领导");
            await CreateUserIfNotExistsAsync("朱丽平", "zlp009", GenderType.Female, supervisionOffice.Id, supervisionOrg.Id, "超级管理员, 部门领导");
            await CreateUserIfNotExistsAsync("安欣", "ax010", GenderType.Male, supervisionOffice.Id, supervisionOrg.Id, "超级管理员");
        }

        /// <summary>
        /// 创建用户（如果不存在）
        /// </summary>
        private async Task CreateUserIfNotExistsAsync(
            string name,
            string userName,
            GenderType gender,
            Guid workUnitId,
            Guid orgId,
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
                    workUnitId,
                    identityUser.Id,
                    gender)
                {
                    OrgId = orgId
                };

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
                        workUnitId,
                        identityUser.Id,
                        gender)
                    {
                        OrgId = orgId
                    };

                    // 保存用户
                    await _userRepository.InsertAsync(user);

                    _logger.LogInformation($"为已存在的Identity用户创建业务用户：{name}");
                }
            }
        }
    }
}
