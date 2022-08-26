using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.WxPusher.Messages;

public class WxPusherMessageSenderTests : AbpWxPusherTestBase
{
    protected IWxPusherMessageSender WxPusherMessageSender { get; }
    public WxPusherMessageSenderTests()
    {
        WxPusherMessageSender = GetRequiredService<IWxPusherMessageSender>();
    }

    [Theory]
    [InlineData("Content from the Xunit unit test. \r\n Click the link at the top to redirect baidu site.")]
    public async virtual Task Send_Text_Test(string content)
    {
        var result = await WxPusherMessageSender
            .SendAsync(
                content,
                contentType: MessageContentType.Text,
                topicIds: new List<int> { 7182 },
                url: "https://www.baidu.com/");

        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThanOrEqualTo(1);
    }

    [Theory]
    [InlineData("<h>Content from the Xunit unit test.</h> <br /> <a href=\"https://www.baidu.com/\">Click to redirect baidu site.</a>")]
    public async virtual Task Send_Html_Test(string content)
    {
        var result = await WxPusherMessageSender
            .SendAsync(
                content,
                contentType: MessageContentType.Html,
                topicIds: new List<int> { 7182 },
                url: "https://www.baidu.com/");

        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThanOrEqualTo(1);
    }

    [Theory]
    [InlineData("**Content from the Xunit unit test.** <br /> <a href=\"https://www.baidu.com/\">Click to redirect baidu site.</a>")]
    public async virtual Task Send_Markdown_Test(string content)
    {
        var result = await WxPusherMessageSender
            .SendAsync(
                content,
                contentType: MessageContentType.Markdown,
                topicIds: new List<int> { 7182 },
                url: "https://www.baidu.com/");

        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThanOrEqualTo(1);
    }
}
