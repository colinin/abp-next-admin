using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace LINGYUN.Abp.PushPlus.Message;

/// <summary>
/// TODO: 接入webhook测试
/// TODO: 接入企业微信测试
/// </summary>
public class PushPlusMessageSenderTests : AbpPushPlusTestBase
{
    protected IPushPlusMessageSender PushPlusMessageSender { get; }
    public PushPlusMessageSenderTests()
    {
        PushPlusMessageSender = GetRequiredService<IPushPlusMessageSender>();
    }

    [Theory]
    [InlineData("Title from the Xunit unit test", "Content from the Xunit unit test.")]
    public async virtual Task Send_To_We_Chat_Test(
        string title,
        string content)
    {
        var result = await PushPlusMessageSender.SendWeChatAsync(title, content);

        result.ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("Title from the Xunit unit test", "Content from the Xunit unit test.")]
    public async virtual Task Send_To_Email_Test(
        string title,
        string content)
    {
        var result = await PushPlusMessageSender.SendEmailAsync(title, content);

        result.ShouldNotBeNullOrWhiteSpace();
    }
}
