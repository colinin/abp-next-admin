using PackageName.CompanyName.ProjectName.Users.Dtos;
using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;
using Xunit;

namespace PackageName.CompanyName.ProjectName.Users
{
    /// <summary>
    /// UserAppService 的单元测试
    /// </summary>
    [Collection("Database")]
    public class UserAppServiceTests : ProjectNameApplicationTestBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IUserManager _userManager;

        public UserAppServiceTests()
        {
            _userAppService = GetRequiredService<IUserAppService>();
            _userManager = GetRequiredService<IUserManager>();
        }

        [Theory]
        [InlineData("testuser1", "Test123456!", true)]
        [InlineData("testuser2", "Test123456!", false)]
        public async Task Should_Create_User(
            string nickName,
            string password,
            bool isActive)
        {
            // Arrange
            var input = new CreateUpdateUserDto
            {
                NickName = nickName,
                Password = password,
                IsActive = isActive
            };

            // Act
            var result = await _userAppService.CreateAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.NickName.ShouldBe(nickName);
            result.IsActive.ShouldBe(isActive);
        }

        [Theory]
        [InlineData("", "Test123456!", "用户名称不能为空")]
        [InlineData("test", "123", "密码长度必须在6-20个字符之间")]
        public async Task Should_Not_Create_User_With_Invalid_Input(string nickName, string password,
            string expectedErrorMessage)
        {
            // Arrange
            var input = new CreateUpdateUserDto
            {
                NickName = nickName,
                Password = password
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _userAppService.CreateAsync(input);
            });

            exception.ValidationErrors.ShouldContain(x => x.ErrorMessage.Contains(expectedErrorMessage));
        }

        [Fact]
        public async Task Should_Get_User_List()
        {
            // Arrange
            await CreateTestUserAsync("testuser1", "Test123456!");
            await CreateTestUserAsync("testuser2", "Test123456!");

            // Act
            var result = await _userAppService.GetListAsync(
                new UserPagedAndSortedResultRequestDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0,
                    Sorting = "NickName"
                });

            // Assert
            result.ShouldNotBeNull();
            result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
            result.Items.ShouldContain(x => x.NickName == "testuser1");
            result.Items.ShouldContain(x => x.NickName == "testuser2");
        }

        [Fact]
        public async Task Should_Filter_Users_By_NickName()
        {
            // Arrange
            await CreateTestUserAsync("testuser1", "Test123456!");
            await CreateTestUserAsync("testuser2", "Test123456!");
            await CreateTestUserAsync("otheruser", "Test123456!");

            // Act
            var result = await _userAppService.GetListAsync(
                new UserPagedAndSortedResultRequestDto
                {
                    MaxResultCount = 10,
                    SkipCount = 0,
                    Sorting = "NickName",
                    NickName = "testuser"
                });

            // Assert
            result.ShouldNotBeNull();
            result.TotalCount.ShouldBe(2);
            result.Items.ShouldContain(x => x.NickName == "testuser1");
            result.Items.ShouldContain(x => x.NickName == "testuser2");
            result.Items.ShouldNotContain(x => x.NickName == "otheruser");
        }

        [Fact]
        public async Task Should_Update_User()
        {
            // Arrange
            var user = await CreateTestUserAsync("updatetest", "Test123456!");
            var updateInput = new CreateUpdateUserDto
            {
                NickName = "updateduser",
                Password = "NewPassword123!",
                ContactInfo = "13800138000",
                Position = "开发工程师",
                IsActive = true
            };

            // Act
            var result = await _userAppService.UpdateAsync(user.Id, updateInput);

            // Assert
            result.ShouldNotBeNull();
            result.NickName.ShouldBe("updateduser");
            result.ContactInfo.ShouldBe("13800138000");
            result.Position.ShouldBe("开发工程师");

            // 验证更新后的用户信息
            var updatedUser = await _userAppService.GetAsync(user.Id);
            updatedUser.NickName.ShouldBe("updateduser");
        }

        [Fact]
        public async Task Should_Not_Update_Non_Existing_User()
        {
            // Arrange
            var input = new CreateUpdateUserDto
            {
                NickName = "testuser",
                Password = "Test123456!"
            };

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await _userAppService.UpdateAsync(Guid.NewGuid(), input);
            });
        }

        [Fact]
        public async Task Should_Delete_User()
        {
            // Arrange
            var user = await CreateTestUserAsync("deletetest", "Test123456!");

            // Act
            await _userAppService.DeleteAsync(user.Id);

            // Assert - 尝试获取已删除的用户应该抛出异常
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await _userAppService.GetAsync(user.Id);
            });
        }

        [Fact]
        public async Task Should_Change_User_Password()
        {
            // Arrange
            var user = await CreateTestUserAsync("passwordtest", "OldPassword123!");

            // Act & Assert
            await _userAppService.ChangePasswordAsync(user.Id, "OldPassword123!", "NewPassword123!");

            // 尝试用新密码登录（这个需要集成测试才能完整测试）
            // 这里我们只是验证方法执行不会抛出异常
        }

        [Fact]
        public async Task Should_Reset_User_Password()
        {
            // Arrange
            var user = await CreateTestUserAsync("resetpasswordtest", "OldPassword123!");

            // Act & Assert
            await _userAppService.ResetPasswordAsync(user.Id, "NewPassword123!");

            // 同样，完整测试需要验证用户能用新密码登录，这需要集成测试
        }

        [Fact]
        public async Task Should_Set_User_Active_Status()
        {
            // Arrange
            var user = await CreateTestUserAsync("activestatustest", "Password123!");

            // Act
            await _userAppService.SetUserActiveStatusAsync(user.Id, false);
            var disabledUser = await _userAppService.GetAsync(user.Id);

            await _userAppService.SetUserActiveStatusAsync(user.Id, true);
            var enabledUser = await _userAppService.GetAsync(user.Id);

            // Assert
            disabledUser.IsActive.ShouldBeFalse();
            enabledUser.IsActive.ShouldBeTrue();
        }

        [Theory]
        [InlineData("13900000000", "工程师")]
        [InlineData("13800000000", "设计师")]
        [InlineData(null, null)]
        public async Task Should_Create_User_With_Optional_Fields(string contactInfo, string position)
        {
            // Arrange
            var input = new CreateUpdateUserDto
            {
                NickName = $"user_{Guid.NewGuid():N}",
                Password = "Test123456!",
                ContactInfo = contactInfo,
                Position = position
            };

            // Act
            var result = await _userAppService.CreateAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.ContactInfo.ShouldBe(contactInfo);
            result.Position.ShouldBe(position);
        }

        private async Task<UserDto> CreateTestUserAsync(string nickName, string password)
        {
            return await WithUnitOfWorkAsync(async () =>
            {
                var input = new CreateUpdateUserDto
                {
                    NickName = nickName,
                    Password = password,
                    IsActive = true
                };

                return await _userAppService.CreateAsync(input);
            });
        }
    }
}