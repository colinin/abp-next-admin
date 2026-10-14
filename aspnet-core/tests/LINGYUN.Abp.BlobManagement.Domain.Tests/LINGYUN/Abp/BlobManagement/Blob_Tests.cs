using Shouldly;
using System;
using Volo.Abp;
using Xunit;

namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// 存储对象
/// </summary>
public class Blob_Tests : AbpBlobManagementDomainTestBase
{
    [Theory]
    [InlineData("..")]
    [InlineData(".")]
    [InlineData("folder/file.txt")]
    [InlineData("folder\\file.txt")]
    [InlineData("x/../y")]
    [InlineData("NUL")]
    [InlineData("file.txt:ads")]
    [InlineData("file.")]
    public void Should_Reject_Unsafe_Names(string name)
    {
        var blob = new TestBlob();

        var exception = Should.Throw<BusinessException>(() => blob.SetName(name));

        exception.Code.ShouldBe(BlobManagementErrorCodes.Blob.InvalidName);
    }

    [Theory]
    [InlineData("file.txt")]
    [InlineData("file name.txt")]
    [InlineData("目录")]
    [InlineData(".hidden")]
    public void Should_Allow_Legitimate_Names(string name)
    {
        var blob = new TestBlob();

        blob.SetName(name);

        blob.Name.ShouldBe(name);
    }

    [Theory]
    [InlineData("/folder/../etc/passwd")]
    [InlineData("../outside/file.txt")]
    [InlineData("C:\\Windows\\Temp\\pwn.txt")]
    [InlineData("\\\\server\\share\\pwn.txt")]
    [InlineData("/folder//../../x")]
    public void Should_Reject_Unsafe_Full_Names(string fullName)
    {
        var blob = new TestBlob();

        var exception = Should.Throw<BusinessException>(() => blob.SetFullName(fullName));

        exception.Code.ShouldBe(BlobManagementErrorCodes.Blob.InvalidName);
    }

    [Theory]
    [InlineData("/file.txt")]
    [InlineData("/folder/file.txt")]
    [InlineData("/folder/sub/目录/文件.txt")]
    [InlineData("/folder with space/file.txt")]
    public void Should_Allow_Legitimate_Full_Names(string fullName)
    {
        var blob = new TestBlob();

        blob.SetFullName(fullName);

        blob.FullName.ShouldBe(fullName);
    }

    [Fact]
    public void Should_Validate_Names_When_Adding_Sub_Blobs()
    {
        var folder = new TestBlob();
        folder.SetType(BlobType.Folder);
        folder.SetName("folder");
        folder.SetFullName("/folder");

        var subBlob = folder.AddBlob(Guid.NewGuid(), "sub.txt", BlobType.File);
        subBlob.FullName.ShouldBe("/folder/sub.txt");

        var exception = Should.Throw<BusinessException>(() =>
            folder.AddBlob(Guid.NewGuid(), "..", BlobType.File));
        exception.Code.ShouldBe(BlobManagementErrorCodes.Blob.InvalidName);
    }

    /// <summary>
    /// 实体构造函数为 internal, 测试用子类暴露受保护构造函数
    /// </summary>
    private sealed class TestBlob : Blob
    {
        public TestBlob()
        {
        }

        public void SetType(BlobType type)
        {
            Type = type;
        }
    }
}
