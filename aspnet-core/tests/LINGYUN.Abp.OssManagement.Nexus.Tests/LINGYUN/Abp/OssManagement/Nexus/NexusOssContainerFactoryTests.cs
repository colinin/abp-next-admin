using Shouldly;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement.Nexus;
public class NexusOssContainerFactoryTests : AbpOssManagementNexusTestsBase
{
    protected IOssContainerFactory OssContainerFactory { get; }

    public NexusOssContainerFactoryTests()
    {
        OssContainerFactory = GetRequiredService<IOssContainerFactory>();
    }

    [Theory]
    [InlineData("/test-repo")]
    public async virtual Task CreateAsync(string containerName)
    {
        var ossContainer = OssContainerFactory.Create();

        await ossContainer.CreateAsync(containerName);
    }

    [Theory]
    [InlineData("/test-repo", "CreateObjectAsync", "/aaa/bbb/ccc")]
    public async virtual Task CreateObjectAsync(string containerName, string objectName, string path = null)
    {
        var textBytes = Encoding.UTF8.GetBytes("CreateObjectAsync");
        using var stream = new MemoryStream();
        await stream.WriteAsync(textBytes, 0, textBytes.Length);

        var ossContainer = OssContainerFactory.Create();
        var createObjectRequest = new CreateOssObjectRequest(
            containerName,
            objectName,
            stream,
            path);

        var oss = await ossContainer.CreateObjectAsync(createObjectRequest);

        oss.ShouldNotBeNull();
    }
}
