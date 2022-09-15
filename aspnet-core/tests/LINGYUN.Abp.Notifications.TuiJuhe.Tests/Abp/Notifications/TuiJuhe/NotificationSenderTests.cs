using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.TuiJuhe;

public class NotificationSenderTests : AbpNotificationsTuiJuheTestBase
{
    protected INotificationSender NotificationSender { get; }

    public NotificationSenderTests()
    {
        NotificationSender = GetRequiredService<INotificationSender>();
    }

    [Theory]
    [InlineData(
        "Form nTest Text",
        "Text content from the Xunit unit test. \r\n Click the link https://www.baidu.com/ at the top to redirect baidu site.")]
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

        await NotificationSender.SendNofiterAsync(
            NotificationsTestsNames.Test1,
            notificationData);
    }

    [Theory]
    [InlineData(
        "Form nTest Html",
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

        await NotificationSender.SendNofiterAsync(
            NotificationsTestsNames.Test2,
            notificationData);
    }

    [Theory]
    [InlineData(
        "Form nTest Markdown",
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

        await NotificationSender.SendNofiterAsync(
            NotificationsTestsNames.Test3,
            notificationData);
    }

    [Theory]
    [InlineData(
        "Form nTest Json",
        "{\"content\":\"Content from the Xunit unit test.\",\"url\":{\"component\":{\"name\":\"a\",\"value\":\"Click to redirect baidu site.\",\"options\":{\"href\":\"https://www.baidu.com/\"}}}}")]
    public async Task Send_Json_Test(
        string title,
        string message)
    {
        var notificationData = new NotificationData();
        notificationData.WriteStandardData(
            title,
            message,
            DateTime.Now,
            "xUnit Test");

        await NotificationSender.SendNofiterAsync(
            NotificationsTestsNames.Test4,
            notificationData);
    }
}
