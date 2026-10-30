using Microsoft.Extensions.Options;
using Shouldly;
using System;
using Volo.Abp.Timing;
using Xunit;

namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// 下载密钥
/// </summary>
public class BlobDownloadKeyGenerator_Tests : AbpBlobManagementDomainTestBase
{
    private IBlobDownloadKeyGenerator DownloadKeyGenerator => GetRequiredService<IBlobDownloadKeyGenerator>();

    private IClock Clock => GetRequiredService<IClock>();

    [Fact]
    public void Should_Generate_And_Parse_Tenant_Key()
    {
        var tenantId = Guid.NewGuid();
        var blob = CreateBlob(tenantId);

        var key = DownloadKeyGenerator.Generate(blob);

        key.ShouldNotBeNullOrWhiteSpace();
        // 下载密钥会作为Url路径, 不能包含需要转义的字符
        key.ShouldNotContain("+");
        key.ShouldNotContain("/");
        key.ShouldNotContain("=");

        var downloadKey = DownloadKeyGenerator.Parse(key);

        downloadKey.ShouldNotBeNull();
        downloadKey!.BlobId.ShouldBe(blob.Id);
        downloadKey.TenantId.ShouldBe(tenantId);
        downloadKey.ExpiresAt.ShouldBeGreaterThan(Clock.Now.ToUniversalTime());
    }

    [Fact]
    public void Should_Generate_And_Parse_Host_Key()
    {
        var blob = CreateBlob(null);

        var key = DownloadKeyGenerator.Generate(blob);

        var downloadKey = DownloadKeyGenerator.Parse(key);

        downloadKey.ShouldNotBeNull();
        downloadKey!.BlobId.ShouldBe(blob.Id);
        downloadKey.TenantId.ShouldBeNull();
    }

    [Fact]
    public void Should_Use_Lifetime_From_Options()
    {
        var blob = CreateBlob(Guid.NewGuid());

        // 默认: 有效期600s, 窗口600s => 过期时间在 [now+600, now+1200) 之间
        var defaultKey = DownloadKeyGenerator.Parse(DownloadKeyGenerator.Generate(blob));
        defaultKey.ShouldNotBeNull();
        defaultKey!.ExpiresAt.ShouldBeGreaterThan(Clock.Now.ToUniversalTime().AddSeconds(595));
        defaultKey.ExpiresAt.ShouldBeLessThan(Clock.Now.ToUniversalTime().AddSeconds(1205));

        // 自定义有效期
        var oneMinuteGenerator = CreateGenerator(
            new AbpBlobManagementOptions
            {
                DownloadKeySecret = TestSecret,
                DownloadKeyLifetime = TimeSpan.FromMinutes(1),
                DownloadKeyWindow = TimeSpan.FromSeconds(10),
            });

        var oneMinuteKey = oneMinuteGenerator.Parse(oneMinuteGenerator.Generate(blob));
        oneMinuteKey.ShouldNotBeNull();
        oneMinuteKey!.ExpiresAt.ShouldBeGreaterThan(Clock.Now.ToUniversalTime().AddSeconds(55));
        oneMinuteKey.ExpiresAt.ShouldBeLessThan(Clock.Now.ToUniversalTime().AddSeconds(75));
    }

    [Fact]
    public void Should_Generate_Same_Key_Within_Window()
    {
        var blob = CreateBlob(Guid.NewGuid());

        var first = DownloadKeyGenerator.Generate(blob);
        var second = DownloadKeyGenerator.Generate(blob);

        // 同一时间窗口内, 多次生成得到相同的密钥
        second.ShouldBe(first);
    }

    [Fact]
    public void Should_Generate_Different_Key_Across_Windows()
    {
        var generator = CreateGenerator(
            new AbpBlobManagementOptions
            {
                DownloadKeySecret = TestSecret,
                DownloadKeyLifetime = TimeSpan.FromMinutes(10),
                DownloadKeyWindow = TimeSpan.FromSeconds(1),
            });

        var blob = CreateBlob(Guid.NewGuid());

        var first = generator.Generate(blob);

        // 跨过窗口边界后再生成
        System.Threading.Thread.Sleep(1100);

        var second = generator.Generate(blob);

        second.ShouldNotBe(first);
    }

    [Fact]
    public void Should_Reject_Tampered_Key()
    {
        var key = DownloadKeyGenerator.Generate(CreateBlob(Guid.NewGuid()));

        var chars = key.ToCharArray();
        // 篡改载荷区域, 保证一定会改变解码后的字节
        var index = chars.Length / 2;
        chars[index] = chars[index] == 'A' ? 'B' : 'A';

        DownloadKeyGenerator.Parse(new string(chars)).ShouldBeNull();
    }

    [Fact]
    public void Should_Reject_Non_Canonical_Key()
    {
        var key = DownloadKeyGenerator.Generate(CreateBlob(Guid.NewGuid()));

        // 末尾是未被使用的填充位, 变更后解码结果相同, 但编码不再是规范形式, 同样要拒绝
        var chars = key.ToCharArray();
        var lastIndex = chars.Length - 1;
        chars[lastIndex] = chars[lastIndex] == 'A' ? 'B' : 'A';

        DownloadKeyGenerator.Parse(new string(chars)).ShouldBeNull();

        // 追加填充符也不是规范编码
        DownloadKeyGenerator.Parse(key + "=").ShouldBeNull();
    }

    [Fact]
    public void Should_Reject_Expired_Key()
    {
        // 窗口1s + 有效期-10s: 过期时间必定落在 (now-10s, now-9s]
        var expiredGenerator = CreateGenerator(
            new AbpBlobManagementOptions
            {
                DownloadKeySecret = TestSecret,
                DownloadKeyLifetime = TimeSpan.FromSeconds(-10),
                DownloadKeyWindow = TimeSpan.FromSeconds(1),
            });

        var key = expiredGenerator.Generate(CreateBlob(Guid.NewGuid()));

        DownloadKeyGenerator.Parse(key).ShouldBeNull();
    }

    [Fact]
    public void Should_Reject_Key_Signed_By_Another_Secret()
    {
        var anotherGenerator = CreateGenerator(
            new AbpBlobManagementOptions
            {
                DownloadKeySecret = "another-download-key-secret",
            });

        var key = anotherGenerator.Generate(CreateBlob(Guid.NewGuid()));

        DownloadKeyGenerator.Parse(key).ShouldBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abc")]
    [InlineData("!!!not-base64!!!")]
    [InlineData("YWJjZGVmZ2hpamtsbW5vcA")]
    public void Should_Reject_Invalid_Key(string key)
    {
        DownloadKeyGenerator.Parse(key).ShouldBeNull();
    }

    private const string TestSecret = "unit-test-download-key-secret";

    private HmacBlobDownloadKeyGenerator CreateGenerator(AbpBlobManagementOptions options)
    {
        return new HmacBlobDownloadKeyGenerator(Options.Create(options), Clock);
    }

    private static TestBlob CreateBlob(Guid? tenantId)
    {
        return new TestBlob(tenantId);
    }

    /// <summary>
    /// 实体构造函数为 internal, 测试用子类暴露受保护成员
    /// </summary>
    private sealed class TestBlob : Blob
    {
        public TestBlob(Guid? tenantId)
        {
            Id = Guid.NewGuid();
            TenantId = tenantId;
            ContainerId = Guid.NewGuid();
            SetName("test.txt");
            SetFullName("/test.txt");
            Type = BlobType.File;
            Size = 1;
            SetProvider("FileSystem");
        }
    }
}
