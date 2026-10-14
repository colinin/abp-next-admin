#nullable enable
using Shouldly;
using Xunit;

namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// 名称校验器
/// </summary>
public class BlobManagementNameValidator_Tests : AbpBlobManagementDomainTestBase
{
    [Theory]
    [InlineData("file.txt", true)]
    [InlineData("目录", true)]
    [InlineData(".hidden", true)]
    [InlineData("a/b", false)]
    [InlineData("a\\b", false)]
    [InlineData("..", false)]
    [InlineData(".", false)]
    [InlineData("a:b", false)]
    [InlineData("a*b", false)]
    [InlineData("file.", false)]
    [InlineData("file ", false)]
    [InlineData("NUL", false)]
    [InlineData("com1.log", false)]
    [InlineData("console.txt", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void Should_Validate_Single_Names(string? name, bool expected)
    {
        BlobManagementNameValidator.IsValidName(name).ShouldBe(expected);
    }

    [Theory]
    [InlineData("/folder/file.txt", true)]
    [InlineData("folder/sub/file.txt", true)]
    [InlineData("/folder with space/file.txt", true)]
    [InlineData("/folder/../file.txt", false)]
    [InlineData("../folder/file.txt", false)]
    [InlineData("/folder/file.txt.", false)]
    [InlineData("C:/folder/file.txt", false)]
    [InlineData("//server/share/file.txt", false)]
    [InlineData("/", false)]
    [InlineData("", false)]
    public void Should_Validate_Name_Paths(string path, bool expected)
    {
        BlobManagementNameValidator.IsValidPath(path).ShouldBe(expected);
    }

    [Theory]
    [InlineData("NUL", true)]
    [InlineData("nul.txt", true)]
    [InlineData("CON", true)]
    [InlineData("aux", true)]
    [InlineData("COM1", true)]
    [InlineData("lpt9.log", true)]
    [InlineData("console.txt", false)]
    [InlineData("com0", false)]
    [InlineData("COM10.txt", false)]
    [InlineData("file.txt", false)]
    public void Should_Detect_Reserved_Device_Names(string name, bool expected)
    {
        BlobManagementNameValidator.IsReservedDeviceName(name).ShouldBe(expected);
    }
}
