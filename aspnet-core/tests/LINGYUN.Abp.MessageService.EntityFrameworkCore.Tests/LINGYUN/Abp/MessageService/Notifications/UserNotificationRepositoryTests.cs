using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using LINGYUN.Abp.Notifications;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class UserNotificationRepositoryTests : AbpMessageServiceEntityFrameworkCoreTestBase
    {
        private readonly IUserNotificationRepository _repository;
        public UserNotificationRepositoryTests()
        {
            _repository = GetRequiredService<IUserNotificationRepository>();
        }

        [Fact]
        public async Task AnyAsync_Test()
        {
            (await _repository.AnyAsync(NotificationsTestConsts.User1Id, NotificationsTestConsts.NotificationId1)).ShouldBeFalse();
            (await _repository.AnyAsync(NotificationsTestConsts.User1Id, NotificationsTestConsts.NotificationId3)).ShouldBeTrue();
        }

        [Fact]
        public async Task GetByIdAsync_Test()
        {
            (await _repository.GetByIdAsync(NotificationsTestConsts.User1Id, NotificationsTestConsts.NotificationId2)).ShouldNotBeNull();
            (await _repository.GetByIdAsync(NotificationsTestConsts.User1Id, NotificationsTestConsts.NotificationId1)).ShouldBeNull();
        }

        [Fact]
        public async Task GetNotificationsAsync_Test()
        {
            (await _repository.GetNotificationsAsync(
                NotificationsTestConsts.User1Id, 
                NotificationReadState.Read))
                .Count
                .ShouldBe(1);
            (await _repository.GetNotificationsAsync(
                NotificationsTestConsts.User1Id,
                NotificationReadState.UnRead))
                .Count
                .ShouldBe(1);
            (await _repository.GetNotificationsAsync(
                NotificationsTestConsts.User1Id))
                .Count
                .ShouldBe(2);
            (await _repository.GetNotificationsAsync(
                NotificationsTestConsts.User2Id,
                NotificationReadState.UnRead))
                .Count
                .ShouldBe(1);
        }

        [Fact]
        public async Task GetCountAsync_Test()
        {
            (await _repository.GetCountAsync(
                NotificationsTestConsts.User1Id,
                readState: NotificationReadState.Read))
                .ShouldBe(1);
            (await _repository.GetCountAsync(
                NotificationsTestConsts.User1Id))
                .ShouldBe(2);
        }

        [Fact]
        public async Task GetListAsync_Test()
        {
            (await _repository.GetListAsync(
                NotificationsTestConsts.User1Id,
                readState: NotificationReadState.Read))
                .Count
                .ShouldBe(1);
            (await _repository.GetListAsync(
                NotificationsTestConsts.User1Id))
                .Count
                .ShouldBe(2);
        }
    }
}
