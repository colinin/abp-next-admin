using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Models;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms;

public class WeChatWorkMeetingRoomProvider_Tests : AbpWeChatWorkOATestBase
{
    private readonly IWeChatWorkMeetingRoomProvider _provider;
    public WeChatWorkMeetingRoomProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkMeetingRoomProvider>();
    }

    [Fact]
    public async virtual Task Should_Create_Meeting_Room()
    {
        var res = await CreateMeetingRoomAsync();

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
        res.MeetingRoomId.ShouldNotBe(0);

        await _provider.DeleteMeetingRoomAsync(
            new WeChatWorkDeleteMeetingRoomRequest(res.MeetingRoomId));
    }

    [Fact]
    public async virtual Task Should_Get_Meeting_Room_List()
    {
        var createRes = await CreateMeetingRoomAsync();

        var req = new WeChatWorkGetMeetingRoomListRequest();

        var res = await _provider.GetMeetingRoomListAsync(req);

        await _provider.DeleteMeetingRoomAsync(
            new WeChatWorkDeleteMeetingRoomRequest(createRes.MeetingRoomId));

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
        res.MeetingRoomList.ShouldNotBeEmpty();

        var meetingRoom = res.MeetingRoomList[0];
        meetingRoom.MeetingRoomId.ShouldNotBe(0);
        meetingRoom.Name.ShouldBe("18F-会议室");
        meetingRoom.Capacity.ShouldBe(10);
        meetingRoom.City.ShouldBe("深圳");
        meetingRoom.Building.ShouldBe("腾讯大厦");
        meetingRoom.Floor.ShouldBe("18F");
        meetingRoom.Equipment.ShouldContain(1);
        meetingRoom.Equipment.ShouldContain(2);
        meetingRoom.Equipment.ShouldContain(3);
        meetingRoom.NeedApproval.ShouldBeFalse();
        meetingRoom.Coordinate.ShouldNotBeNull();
        meetingRoom.Coordinate.Latitude.ShouldBe("22.540503");
        meetingRoom.Coordinate.Longitude.ShouldBe("113.934528");
        meetingRoom.Range.ShouldBeNull();
    }

    [Fact]
    public async virtual Task Should_Update_Meeting_Room()
    {
        var createRes = await CreateMeetingRoomAsync();

        var req = new WeChatWorkUpdateMeetingRoomRequest(createRes.MeetingRoomId);
        req.Name = "18F-会议室1";
        req.WithWhiteboard();

        var res = await _provider.UpdateMeetingRoomAsync(req);

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");

        var getRes = await _provider.GetMeetingRoomListAsync(
            new WeChatWorkGetMeetingRoomListRequest());

        await _provider.DeleteMeetingRoomAsync(
            new WeChatWorkDeleteMeetingRoomRequest(createRes.MeetingRoomId));

        var meetingRoom = getRes.MeetingRoomList[0];
        meetingRoom.Name.ShouldBe("18F-会议室1");
        meetingRoom.Equipment.ShouldContain(4);
    }

    [Fact]
    public async virtual Task Should_Delete_Meeting_Room()
    {
        var createRes = await CreateMeetingRoomAsync();

        var res = await _provider.DeleteMeetingRoomAsync(
            new WeChatWorkDeleteMeetingRoomRequest(createRes.MeetingRoomId));

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }

    [Fact]
    public async virtual Task Should_Book_Meeting_Room()
    {
        var createRes = await CreateMeetingRoomAsync();

        var configuration = GetRequiredService<IConfiguration>();

        var nowTime = DateTime.Now;

        var res = await _provider.BookMeetingRoomAsync(
            new WeChatWorkBookMeetingRoomRequest(
                createRes.MeetingRoomId,
                nowTime.AddHours(2),
                nowTime.AddHours(4),
                configuration["WeChat:Work:OA:MeetingRooms:BookMeetingRoom:Booker"]));

        await _provider.CancelBookMeetingRoomAsync(
            new WeChatWorkCancelBookMeetingRoomRequest(res.BookingId));

        await _provider.DeleteMeetingRoomAsync(
            new WeChatWorkDeleteMeetingRoomRequest(createRes.MeetingRoomId));

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
        res.BookingId.ShouldNotBeNull();
        res.ScheduleId.ShouldNotBeNull();
    }

    [Fact]
    public async virtual Task Should_Get_Meeting_Room_Book()
    {
        var createRes = await CreateMeetingRoomAsync();

        var configuration = GetRequiredService<IConfiguration>();

        var nowTime = DateTime.Now;

        var bookRes = await _provider.BookMeetingRoomAsync(
            new WeChatWorkBookMeetingRoomRequest(
                createRes.MeetingRoomId,
                nowTime.AddHours(2),
                nowTime.AddHours(4),
                configuration["WeChat:Work:OA:MeetingRooms:BookMeetingRoom:Booker"]));

        var res = await _provider.GetMeetingRoomBookAsync(
            new WeChatWorkGetMeetingRoomBookRequest(createRes.MeetingRoomId, bookRes.BookingId));

        await _provider.CancelBookMeetingRoomAsync(
            new WeChatWorkCancelBookMeetingRoomRequest(bookRes.BookingId));

        await _provider.DeleteMeetingRoomAsync(
            new WeChatWorkDeleteMeetingRoomRequest(createRes.MeetingRoomId));

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
        res.Schedule.ShouldNotBeNull();
        res.Schedule.Booker.ShouldBe(configuration["WeChat:Work:OA:MeetingRooms:BookMeetingRoom:Booker"]);
        res.Schedule.Status.ShouldBe(MeetingRoomScheduleStatus.Reserved);
        res.Schedule.BookingId.ShouldBe(bookRes.BookingId);
        res.Schedule.ScheduleId.ShouldBe(bookRes.ScheduleId);
    }

    protected async virtual Task<WeChatWorkCreateMeetingRoomResponse> CreateMeetingRoomAsync()
    {
        var req = new WeChatWorkCreateMeetingRoomRequest("18F-会议室", 10);
        req.WithLocation("深圳", "腾讯大厦", "18F");
        req.WithTv();
        req.WithPhone();
        req.WithProjection();
        req.Coordinate = new MeetingRoomCoordinate(22.540503m, 113.934528m);

        return await _provider.CreateMeetingRoomAsync(req);
    }
}
