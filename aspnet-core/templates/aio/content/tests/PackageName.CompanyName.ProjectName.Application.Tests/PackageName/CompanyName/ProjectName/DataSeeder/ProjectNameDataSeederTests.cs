using Microsoft.Extensions.DependencyInjection;
using PackageName.CompanyName.ProjectName.AIO.EntityFrameworkCore.DataSeeder;
using PackageName.CompanyName.ProjectName.Users;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Xunit;

namespace PackageName.CompanyName.ProjectName.DataSeeder
{
    /// <summary>
    /// 数据种子初始化测试
    /// </summary>
    [Collection("Database")]
    public class ProjectNameDataSeederTests : ProjectNameApplicationTestBase
    {
        private readonly IProjectNameDataSeeder _inspectionDataSeeder;
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IIdentityRoleRepository _identityRoleRepository;
        private readonly IIdentityUserRepository _identityUserRepository;

        public ProjectNameDataSeederTests()
        {
            _inspectionDataSeeder = GetRequiredService<IProjectNameDataSeeder>();
            _userRepository = GetRequiredService<IRepository<User, Guid>>();
            _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
            _identityUserRepository = GetRequiredService<IIdentityUserRepository>();
        }

        [Fact]
        public async Task Should_Seed_Data_Successfully()
        {
            // Arrange
            var context = new DataSeedContext();

            // Act
            await _inspectionDataSeeder.SeedAsync(context);

            // Assert - 使用单元工作方法包装所有数据库操作
            await WithUnitOfWorkAsync(async () =>
            {
                // 测试角色
                var roles = await _identityRoleRepository.GetListAsync();
                roles.Count.ShouldBeGreaterThanOrEqualTo(7); // 至少应该有 7 个角色

                var superAdminRole = await _identityRoleRepository.FindByNormalizedNameAsync("超级管理员".ToUpperInvariant());
                superAdminRole.ShouldNotBeNull();
                // 测试用户
                var users = await _userRepository.GetListAsync();
                users.Count.ShouldBeGreaterThanOrEqualTo(10); // 至少应该有 10 个用户

                foreach (var user in users)
                {
                    user.IdentityUserId.ShouldNotBe(Guid.Empty);

                    var identityUser = await _identityUserRepository.GetAsync(user.IdentityUserId);
                    identityUser.ShouldNotBeNull();
                }
                
                return true;
            });
        }

        [Theory]
        [InlineData("超级管理员")]
        [InlineData("普通用户")]
        public async Task Should_Create_Roles(string roleName)
        {
            // Arrange
            var context = new DataSeedContext();
            await _inspectionDataSeeder.SeedAsync(context);

            // Act & Assert - 使用单元工作方法包装
            await WithUnitOfWorkAsync(async () =>
            {
                var role = await _identityRoleRepository.FindByNormalizedNameAsync(roleName.ToUpperInvariant());
                role.ShouldNotBeNull();
                role.Name.ShouldBe(roleName);
                
                return true;
            });
        }

        [Theory]
        [InlineData("testuser1")]
        [InlineData("testuser2")]
        public async Task Should_Create_Users(string nickName)
        {
            // Arrange
            var context = new DataSeedContext();
            await _inspectionDataSeeder.SeedAsync(context);

            // Act & Assert - 使用单元工作方法包装
            await WithUnitOfWorkAsync(async () =>
            {
                var users = await _userRepository.GetListAsync();
                var user = users.FirstOrDefault(u => u.NickName == nickName);
                user.ShouldNotBeNull();
                user.NickName.ShouldBe(nickName);
                user.IdentityUserId.ShouldNotBe(Guid.Empty);

                var identityUser = await _identityUserRepository.GetAsync(user.IdentityUserId);
                identityUser.ShouldNotBeNull();
                identityUser.Name.ShouldBe(nickName);
                
                return true;
            });
        }
        
        // 添加单元工作方法
        protected override Task<TResult> WithUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
        {
            return WithUnitOfWorkAsync(new AbpUnitOfWorkOptions(), func);
        }
        
        // 可选：添加重载方法以支持更多场景
        protected async override Task<TResult> WithUnitOfWorkAsync<TResult>(AbpUnitOfWorkOptions options, Func<Task<TResult>> func)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin(options))
                {
                    var result = await func();
                    await uow.CompleteAsync();
                    return result;
                }
            }
        }
    }
}
