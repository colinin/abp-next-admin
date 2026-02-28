using LINGYUN.Abp.WeChat.Work.Common.Messages;
using Microsoft.Extensions.Options;

namespace LINGYUN.Abp.WeChat.Work.OA.Messages;

public class AbpWeChatWorkMessageResolveOptions_Tests : AbpWeChatWorkOATestBase
{
    private readonly IOptions<AbpWeChatWorkMessageResolveOptions> _options;
    public AbpWeChatWorkMessageResolveOptions_Tests()
    {
        _options = GetRequiredService<IOptions<AbpWeChatWorkMessageResolveOptions>>();
    }

    [Theory]
    [InlineData("book_meeting_room")]
    [InlineData("cancel_meeting_room")]
    [InlineData("delete_calendar")]
    [InlineData("modify_calendar")]
    [InlineData("delete_schedule")]
    [InlineData("modify_schedule")]
    [InlineData("respond_schedule")]
    public void Should_Map_Event(string eventName)
    {
        _options.Value.EventMaps.ContainsKey(eventName).ShouldBeTrue();
    }
}
