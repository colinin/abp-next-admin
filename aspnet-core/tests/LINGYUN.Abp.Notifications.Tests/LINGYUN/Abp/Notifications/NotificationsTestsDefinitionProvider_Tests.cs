using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Xunit;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationsTestsDefinitionProvider_Tests : AbpNotificationsTestsBase
    {
        protected INotificationDefinitionManager NotificationDefinitionManager { get; }

        public NotificationsTestsDefinitionProvider_Tests()
        {
            NotificationDefinitionManager = GetRequiredService<INotificationDefinitionManager>();
        }

        [Fact]
        public async Task GetGroups_Test()
        {
            var groups = await NotificationDefinitionManager.GetGroupsAsync();
            groups.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetNotifications_Test()
        {
            var notifications = await NotificationDefinitionManager.GetNotificationsAsync();
            notifications.Count.ShouldBe(6);
        }

        [Fact]
        public async Task GetOrNull_Test()
        {
            (await NotificationDefinitionManager.GetOrNullAsync(NotificationsTestsNames.Test2)).ShouldNotBeNull();
            (await NotificationDefinitionManager.GetOrNullAsync(NotificationsTestsNames.Test3)).ShouldNotBeNull();
            (await NotificationDefinitionManager.GetOrNullAsync("NullOfNotification")).ShouldBeNull();
        }

        [Theory]
        [InlineData(NotificationsTestsNames.Test3)]
        public async Task Get_Test(string name)
        {
            var notification = await NotificationDefinitionManager.GetAsync(name);
            notification.Name.ShouldBe(name);
            notification.DisplayName.ShouldBeOfType<FixedLocalizableString>();
            notification.Description.ShouldBeNull();
            notification.AllowSubscriptionToClients.ShouldBeFalse();
            notification.NotificationLifetime.ShouldBe(NotificationLifetime.OnlyOne);
            notification.NotificationType.ShouldBe(NotificationType.User);
            notification.ContentType.ShouldBe(NotificationContentType.Markdown);
        }
    }
}
