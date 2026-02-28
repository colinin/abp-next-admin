using LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Response;
using Shouldly;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.OA.Schedules;

public class WeChatWorkScheduleProvider_Tests : AbpWeChatWorkOATestBase
{
    private readonly static DateTime _beginUnixTime = new DateTime(1970, 1, 1);

    private readonly IWeChatWorkScheduleProvider _provider;
    public WeChatWorkScheduleProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkScheduleProvider>();
    }

    [Fact]
    public async virtual Task Should_Create_Schedule()
    {
        var res = await CreateSchedule();

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
        res.ScheduleId.ShouldNotBeNullOrWhiteSpace();

        await _provider.DeleteScheduleAsync(
            new WeChatWorkDeleteScheduleRequest(res.ScheduleId));
    }

    [Fact]
    public async virtual Task Should_Update_Schedule()
    {
        var createRes = await CreateSchedule();

        var req = new WeChatWorkUpdateScheduleRequest(
            new Models.UpdateSchedule(
                createRes.ScheduleId,
                DateTime.Now.AddHours(5),
                DateTime.Now.AddHours(8)));
        var repeatUntil = (long)(DateTime.Now.AddHours(10) - _beginUnixTime).TotalSeconds;
        req.Schedule.Summary = "test_summary";
        req.Schedule.Description = "test_description";
        req.Schedule.Location = "test_place";
        req.Schedule.Reminders = new Models.ScheduleReminder
        {
            IsRemind = true,
            RemindBeforeEventSecs = 3600u,
            RemindTimeDiffs = new[] { 0, -300 },
            IsRepeat = true,
            RepeatType = Models.ScheduleReminderRepeatType.WorkingDays,
            RepeatUntil = repeatUntil,
            IsCustomRepeat = true,
            RepeatInterval = 1,
            RepeatDayOfWeek = new[] { 3u, 7u },
            RepeatDayOfMonth = new[] { 10u, 21u },
            TimeZone = 8u,
        };

        var res = await _provider.UpdateScheduleAsync(req);

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
        res.ScheduleId.ShouldNotBeNullOrWhiteSpace();

        var getRes = await _provider.GetScheduleListAsync(
            new WeChatWorkGetScheduleListRequest(new[] { res.ScheduleId } ));

        getRes.ErrorCode.ShouldBe(0);
        getRes.ErrorMessage.ShouldBe("ok");
        getRes.ScheduleList.ShouldNotBeEmpty();

        var schedule = getRes.ScheduleList[0];
        schedule.Summary.ShouldBe("test_summary");
        schedule.Description.ShouldBe("test_description");
        schedule.Location.ShouldBe("test_place");
        schedule.Reminders.ShouldNotBeNull();
        schedule.Reminders.IsRemind.ShouldBeTrue();
        schedule.Reminders.RemindTimeDiffs.ShouldContain(0);
        schedule.Reminders.RemindTimeDiffs.ShouldContain(-300);
        schedule.Reminders.IsRepeat.ShouldBeTrue();
        schedule.Reminders.RepeatType.ShouldBe(Models.ScheduleReminderRepeatType.WorkingDays);
        schedule.Reminders.RepeatUntil.ShouldBe(repeatUntil);
        schedule.Reminders.IsCustomRepeat.ShouldBeTrue();
        schedule.Reminders.RepeatInterval.ShouldBe(1u);
        schedule.Reminders.TimeZone.ShouldBe(8u);

        await _provider.DeleteScheduleAsync(
            new WeChatWorkDeleteScheduleRequest(createRes.ScheduleId));
    }

    protected async virtual Task<WeChatWorkCreateScheduleResponse> CreateSchedule()
    {
        var req = new WeChatWorkCreateScheduleRequest(
            new Models.CreateSchedule(
                DateTime.Now.AddHours(2),
                DateTime.Now.AddHours(5)));

        req.Schedule.Summary = "需求评审会议";
        req.Schedule.Description = "2.0版本需求初步评审";
        req.Schedule.Location = "广州国际媒体港10楼1005会议室";
        req.Schedule.IsWholeDay = true;
        req.Schedule.Reminders = new Models.ScheduleReminder
        {
            IsRemind = true,
            RemindBeforeEventSecs = 3600,
            RemindTimeDiffs = new[] { 0, -3600 },
            IsRepeat = true,
            RepeatType = Models.ScheduleReminderRepeatType.WorkingDays,
            RepeatUntil = (long)(DateTime.Now.AddHours(8) - _beginUnixTime).TotalSeconds,
            IsCustomRepeat = true,
            RepeatInterval = 1,
            RepeatDayOfWeek = new[] { 3u, 7u },
            RepeatDayOfMonth = new[] { 10u, 21u },
            TimeZone = 8u,
        };

        return await _provider.CreateScheduleAsync(req);
    }
}
