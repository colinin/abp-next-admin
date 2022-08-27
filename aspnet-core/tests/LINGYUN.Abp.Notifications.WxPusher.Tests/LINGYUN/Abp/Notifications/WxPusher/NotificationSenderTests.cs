using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.WxPusher;

public class NotificationSenderTests : AbpNotificationsWxPusherTestBase
{
    protected INotificationSender NotificationSender { get; }

    public NotificationSenderTests()
    {
        NotificationSender = GetRequiredService<INotificationSender>();
    }

    [Theory]
    [InlineData(
        "Title from the Xunit unit test",
        "Text content from the Xunit unit test. \r\n Click the link at the top to redirect baidu site.")]
    public async Task Send_Text_Test(
        string title,
        string message)
    {
        var notificationData = new NotificationData();
        notificationData.WriteStandardData(
            title,
            message,
            DateTime.Now,
            "xUnit Test");
        notificationData.SetUrl("https://www.baidu.com/");

        await NotificationSender.SendNofiterAsync(
            NotificationsTestsNames.Test1,
            notificationData);
    }

    [Theory]
    [InlineData(
        "Title from the Xunit unit test",
        "<h>Html content from the Xunit unit test.</h> <br /> <a href=\"https://www.baidu.com/\">Click to redirect baidu site.</a>")]
    public async Task Send_Html_Test(
        string title,
        string message)
    {
        var notificationData = new NotificationData();
        notificationData.WriteStandardData(
            title,
            message,
            DateTime.Now,
            "xUnit Test");
        notificationData.SetUrl("https://www.baidu.com/");

        await NotificationSender.SendNofiterAsync(
            NotificationsTestsNames.Test2,
            notificationData);
    }

    [Theory]
    [InlineData(
        "Title from the Xunit unit test",
        "**Markdown content from the Xunit unit test.** <br /> <a href=\"https://www.baidu.com/\">Click to redirect baidu site.</a>")]
    public async Task Send_Markdown_Test(
        string title,
        string message)
    {
        var notificationData = new NotificationData();
        notificationData.WriteStandardData(
            title,
            message,
            DateTime.Now,
            "xUnit Test");
        notificationData.SetUrl("https://www.baidu.com/");

        await NotificationSender.SendNofiterAsync(
            NotificationsTestsNames.Test3,
            notificationData);
    }
}
