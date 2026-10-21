using Shouldly;
using System;
using Volo.Abp;
using Xunit;

namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// 存储容器
/// </summary>
public class BlobContainer_Tests : AbpBlobManagementDomainTestBase
{
    [Theory]
    [InlineData("..")]
    [InlineData(".")]
    [InlineData("a/b")]
    [InlineData("a\\b")]
    [InlineData("x/../abp-c")]
    [InlineData("..\\..\\evil")]
    [InlineData("NUL")]
    [InlineData("con.txt")]
    [InlineData("COM1")]
    [InlineData("a:b")]
    [InlineData("a*b")]
    [InlineData("trailing.")]
    [InlineData("trailing ")]
    public void Should_Reject_Unsafe_Names(string name)
    {
        var container = new TestBlobContainer();

        var exception = Should.Throw<BusinessException>(() => container.SetName(name));

        exception.Code.ShouldBe(BlobManagementErrorCodes.Container.InvalidName);
        exception.Data["Name"].ShouldBe(name);
    }

    [Theory]
    [InlineData("public")]
    [InlineData("users")]
    [InlineData("my-container")]
    [InlineData("my_container.1")]
    [InlineData("对象存储")]
    public void Should_Allow_Legitimate_Names(string name)
    {
        var container = new TestBlobContainer();

        container.SetName(name);

        container.Name.ShouldBe(name);
    }

    /// <summary>
    /// 实体构造函数为 internal, 测试用子类暴露受保护构造函数
    /// </summary>
    private sealed class TestBlobContainer : BlobContainer
    {
        public TestBlobContainer()
        {
        }
    }
}
