using LINGYUN.Abp.WeChat.Work.ExternalContact.Tags.Request;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Tags;
public class WeChatWorkCropTagProvider_Tests : AbpWeChatWorkExternalContactTestBase
{
    public readonly IWeChatWorkCropTagProvider _provider;
    public readonly IConfiguration _configuration;
    public WeChatWorkCropTagProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkCropTagProvider>();
        _configuration = GetRequiredService<IConfiguration>();
    }

    [Fact]
    public async virtual Task Should_Get_Crop_Tag_List()
    {
        var res = await _provider.GetCropTagListAsync(
            new WeChatWorkGetCropTagListRequest());

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }

    [Fact]
    public async virtual Task Should_Create_Crop_Tag()
    {
        var res = await _provider.CreateCropTagAsync(
            new WeChatWorkCreateCropTagRequest()
            {
                GroupName = "test",
            });

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");

        await _provider.DeleteCropTagAsync(
            WeChatWorkDeleteCropTagRequest.Group(new string[1] { res.TagGroup.GroupId }));
    }

    [Fact]
    public async virtual Task Should_Update_Crop_Tag()
    {
        var createRes = await _provider.CreateCropTagAsync(
            new WeChatWorkCreateCropTagRequest()
            {
                GroupName = "test",
            });

        var updateRes = await _provider.UpdateCropTagAsync(
            new WeChatWorkUpdateCropTagRequest(createRes.TagGroup.GroupId, "test_update"));

        updateRes.ErrorCode.ShouldBe(0);
        updateRes.ErrorMessage.ShouldBe("ok");

        await _provider.DeleteCropTagAsync(
            WeChatWorkDeleteCropTagRequest.Group(new string[1] { createRes.TagGroup.GroupId }));
    }

    [Fact]
    public async virtual Task Should_Delete_Crop_Tag()
    {
        var createRes = await _provider.CreateCropTagAsync(
            new WeChatWorkCreateCropTagRequest()
            {
                GroupName = "test",
            });

        var deleteRes = await _provider.DeleteCropTagAsync(
            WeChatWorkDeleteCropTagRequest.Group(new string[1] { createRes.TagGroup.GroupId }));

        deleteRes.ErrorCode.ShouldBe(0);
        deleteRes.ErrorMessage.ShouldBe("ok");
    }

    [Fact]
    public async virtual Task Should_Mark_Crop_Tag()
    {
        var userId = _configuration["WeChat:Work:ExternalContact:Tags:MarkCropTag:UserId"];
        var externalUserId = _configuration["WeChat:Work:ExternalContact:Tags:MarkCropTag:ExternalUserId"];

        var req = new WeChatWorkMarkCropTagRequest(userId, externalUserId);
        req.CreateTag.Add("test_tag");
        var res = await _provider.MarkCropTagAsync(req);

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }
}
