using System.Threading.Tasks;

namespace LINGYUN.Abp.TuiJuhe.Messages;

public class TuiJuheMessageSenderTests : AbpTuiJuheTestBase
{
    protected ITuiJuheMessageSender MessageSender { get; }
    public TuiJuheMessageSenderTests()
    {
        MessageSender = GetRequiredService<ITuiJuheMessageSender>();
    }

    [Theory]
    [InlineData(
        "Form Test Text",
        "Content from the xUnit unit test. \r\n Click the link https://www.baidu.com/ at the top to redirect baidu site.",
        "1d5v5GuH")]
    public async virtual Task Send_Text_Test(string title, string content, string serviceId)
    {
        var result = await MessageSender
            .SendAsync(
                title,
                content,
                serviceId,
                MessageContentType.Text);

        result.ShouldNotBeNull();
        result.Code.ShouldBe(200);
        result.Reason.ShouldBe("success");
        result.Success.ShouldBeTrue();
        result.Params.ShouldBeNull();
        result.Result.ShouldBeNull();
    }

    [Theory]
    [InlineData(
        "Form Test Html",
        "<h>Content from the xUnit unit test.</h> <br /> <a href=\"https://www.baidu.com/\">Click to redirect baidu site.</a>",
        "1d5v5GuH")]
    public async virtual Task Send_Html_Test(string title, string content, string serviceId)
    {
        var result = await MessageSender
            .SendAsync(
                title,
                content,
                serviceId,
                MessageContentType.Html);

        result.ShouldNotBeNull();
        result.Code.ShouldBe(200);
        result.Reason.ShouldBe("success");
        result.Success.ShouldBeTrue();
        result.Params.ShouldBeNull();
        result.Result.ShouldBeNull();
    }

    [Theory]
    [InlineData(
        "Form Test Markdown",
        "**Content from the Xunit unit test.** <br /> <a href=\"https://www.baidu.com/\">Click to redirect baidu site.</a>",
        "1d5v5GuH")]
    public async virtual Task Send_Markdown_Test(string title, string content, string serviceId)
    {
        var result = await MessageSender
             .SendAsync(
                 title,
                 content,
                 serviceId,
                 MessageContentType.Markdown);

        result.ShouldNotBeNull();
        result.Code.ShouldBe(200);
        result.Reason.ShouldBe("success");
        result.Success.ShouldBeTrue();
        result.Params.ShouldBeNull();
        result.Result.ShouldBeNull();
    }

    [Theory]
    [InlineData(
        "Form Test Json",
        "{\"content\":\"Content from the Xunit unit test.\",\"url\":{\"component\":{\"name\":\"a\",\"value\":\"Click to redirect baidu site.\",\"options\":{\"href\":\"https://www.baidu.com/\"}}}}",
        "1d5v5GuH")]
    public async virtual Task Send_Json_Test(string title, string content, string serviceId)
    {
        var result = await MessageSender
             .SendAsync(
                 title,
                 content,
                 serviceId,
                 MessageContentType.Json);

        result.ShouldNotBeNull();
        result.Code.ShouldBe(200);
        result.Reason.ShouldBe("success");
        result.Success.ShouldBeTrue();
        result.Params.ShouldBeNull();
        result.Result.ShouldBeNull();
    }
}
