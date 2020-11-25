using Shouldly;
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
        public void GetGroups_Test()
        {
            var groups = NotificationDefinitionManager.GetGroups();
            groups.Count.ShouldBe(1);
        }

        [Fact]
        public void GetAll_Test()
        {
            var notifications = NotificationDefinitionManager.GetAll();
            notifications.Count.ShouldBe(3);
        }

        [Fact]
        public void GetOrNull_Test()
        {
            NotificationDefinitionManager.GetOrNull(NotificationsTestsNames.Test2).ShouldNotBeNull();
            NotificationDefinitionManager.GetOrNull(NotificationsTestsNames.Test3).ShouldNotBeNull();
            NotificationDefinitionManager.GetOrNull("NullOfNotification").ShouldBeNull();
        }

        [Theory]
        [InlineData(NotificationsTestsNames.Test1)]
        public void Get_Test(string name)
        {
            var notification = NotificationDefinitionManager.Get(name);
            notification.Name.ShouldBe(name);
            notification.DisplayName.ShouldBeOfType<FixedLocalizableString>();
            notification.Description.ShouldBeNull();
            notification.AllowSubscriptionToClients.ShouldBeFalse();
            notification.NotificationLifetime.ShouldBe(NotificationLifetime.OnlyOne);
            notification.NotificationType.ShouldBe(NotificationType.Application);
        }
    }
}
