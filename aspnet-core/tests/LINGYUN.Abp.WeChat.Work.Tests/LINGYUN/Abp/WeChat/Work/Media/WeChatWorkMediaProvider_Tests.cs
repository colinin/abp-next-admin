using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace LINGYUN.Abp.WeChat.Work.Media;
public class WeChatWorkMediaProvider_Tests : AbpWeChatWorkTestBase
{
    protected IWeChatWorkMediaProvider MediaProvider { get; }
    public WeChatWorkMediaProvider_Tests()
    {
        MediaProvider = GetRequiredService<IWeChatWorkMediaProvider>();
    }

    [Theory]
    [InlineData("D:\\Projects\\Development\\Abp\\WeChat\\Work\\image.jpg")]
    public async Task Get_Media_Test(string fileName)
    {
        var fileInfo = new FileInfo(fileName);
        using var fileStream = fileInfo.OpenRead();
        var uploadMedia = new RemoteStreamContent(fileStream, fileInfo.Name);

        var uploadResponse = await MediaProvider.UploadAsync("image", uploadMedia);
        uploadResponse.IsSuccessed.ShouldBeTrue();
        uploadResponse.MediaId.ShouldNotBeNullOrEmpty();

        var getResponse = await MediaProvider.GetAsync(uploadResponse.MediaId);
        getResponse.ShouldNotBeNull();
        getResponse.ContentLength.ShouldNotBeNull();
        getResponse.ContentLength.ShouldBe(uploadMedia.ContentLength);
    }

    [Theory]
    [InlineData("image", "D:\\Projects\\Development\\Abp\\WeChat\\Work\\image.jpg")]
    [InlineData("voice", "D:\\Projects\\Development\\Abp\\WeChat\\Work\\voice.amr")]
    [InlineData("video", "D:\\Projects\\Development\\Abp\\WeChat\\Work\\video.mp4")]
    [InlineData("file", "D:\\Projects\\Development\\Abp\\WeChat\\Work\\file.txt")]
    public async Task Upload_Media_Test(string type, string fileName)
    {
        var fileInfo = new FileInfo(fileName);
        using var fileStream = fileInfo.OpenRead();
        var media = new RemoteStreamContent(fileStream, fileInfo.Name);

        var response = await MediaProvider.UploadAsync(type, media);
        response.IsSuccessed.ShouldBeTrue();
        response.MediaId.ShouldNotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("D:\\Projects\\Development\\Abp\\WeChat\\Work\\image.jpg")]
    public async Task Upload_Image_Test( string fileName)
    {
        var fileInfo = new FileInfo(fileName);
        using var fileStream = fileInfo.OpenRead();
        var media = new RemoteStreamContent(fileStream, fileInfo.Name);

        var response = await MediaProvider.UploadImageAsync(media);
        response.IsSuccessed.ShouldBeTrue();
        response.Url.ShouldNotBeNullOrEmpty();
    }
}
