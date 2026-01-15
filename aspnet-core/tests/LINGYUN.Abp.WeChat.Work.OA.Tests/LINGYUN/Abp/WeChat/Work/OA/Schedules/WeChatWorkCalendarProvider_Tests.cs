using LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules;

public class WeChatWorkCalendarProvider_Tests : AbpWeChatWorkOATestBase
{
    private readonly IWeChatWorkCalendarProvider _provider;
    public WeChatWorkCalendarProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkCalendarProvider>();
    }

    [Fact]
    public async virtual Task Should_Create_Calendar()
    {
        var req = new WeChatWorkCreateCalendarRequest(
            new Models.CreateCalendar(
                "test_summary",
                "#FF3030",
                "test_describe"));

        var res = await _provider.CreateCalendarAsync(req);

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
        res.CalId.ShouldNotBeNullOrWhiteSpace();
        res.FailResult.ShouldNotBeNull();
        res.FailResult.Shares.ShouldBeEmpty();

        await _provider.DeleteCalendarAsync(
            new WeChatWorkDeleteCalendarRequest(res.CalId));
    }

    [Fact]
    public async virtual Task Should_Update_Calendar()
    {
        var createReq = new WeChatWorkCreateCalendarRequest(
            new Models.CreateCalendar(
                "test_summary",
                "#FF3030",
                "test_describe"));

        var createRes = await _provider.CreateCalendarAsync(createReq);

        var req = new WeChatWorkUpdateCalendarRequest(
            new Models.UpdateCalendar(
                createRes.CalId,
                "test_summary1",
                "#FF0000",
                "test_describe1"));

        var res = await _provider.UpdateCalendarAsync(req);

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
        res.FailResult.ShouldNotBeNull();
        res.FailResult.Shares.ShouldBeEmpty();

        var getRes = await _provider.GetCalendarListAsync(
            new WeChatWorkGetCalendarListRequest(new string[] { createRes.CalId }));

        getRes.ErrorCode.ShouldBe(0);
        getRes.ErrorMessage.ShouldBe("ok");
        getRes.CalendarList.ShouldNotBeEmpty();

        var calendar = getRes.CalendarList[0];
        calendar.CalId.ShouldBe(createRes.CalId);
        calendar.Admins.ShouldBeEmpty();
        calendar.Summary.ShouldBe("test_summary1");
        calendar.Color.ShouldBe("#FF0000");
        calendar.Description.ShouldBe("test_describe1");
        calendar.IsPublic.ShouldBeFalse();
        calendar.PublicRange.ShouldBeNull();
        calendar.IsCorpCalendar.ShouldBeFalse();
        calendar.Shares.ShouldBeEmpty();

        await _provider.DeleteCalendarAsync(
            new WeChatWorkDeleteCalendarRequest(createRes.CalId));
    }

    [Fact]
    public async virtual Task Should_Get_Calendar_List()
    {
        var createReq = new WeChatWorkCreateCalendarRequest(
            new Models.CreateCalendar(
                "test_summary",
                "#FF3030",
                "test_describe"));

        var createRes = await _provider.CreateCalendarAsync(createReq);

        var res = await _provider.GetCalendarListAsync(
            new WeChatWorkGetCalendarListRequest(new string[] { createRes.CalId }));

        await _provider.DeleteCalendarAsync(
            new WeChatWorkDeleteCalendarRequest(createRes.CalId));

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
        res.CalendarList.ShouldNotBeEmpty();

        var calendar = res.CalendarList[0];
        calendar.CalId.ShouldBe(createRes.CalId);
        calendar.Admins.ShouldBeEmpty();
        calendar.Summary.ShouldBe("test_summary");
        calendar.Color.ShouldBe("#FF3030");
        calendar.Description.ShouldBe("test_describe");
        calendar.IsPublic.ShouldBeFalse();
        calendar.PublicRange.ShouldBeNull();
        calendar.IsCorpCalendar.ShouldBeFalse();
        calendar.Shares.ShouldBeEmpty();
    }
}
