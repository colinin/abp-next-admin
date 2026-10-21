using Shouldly;
using System;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace LINGYUN.Abp.BlobManagement.FileSystem;

/// <summary>
/// FileSystem provider 路径安全
/// </summary>
/// <remarks>
/// 覆盖目录穿越、绝对路径注入、NTFS 备用数据流、Windows 归一化碰撞、保留设备名、
/// 容器名穿越与容器别名、多租户越权; 同时确认合法名称不受影响。
/// provider 由生产模块按 BlobManagement:Provider=FileSystem 注册, 测试从 DI 解析
/// </remarks>
public class FileSystemBlobProviderSecurity_Tests : AbpBlobManagementFileSystemTestBase
{
    private const string ContainerName = "abp-c";

    private static string BasePath => AbpBlobManagementFileSystemTestsModule.BasePath;

    private static string OutsidePath => AbpBlobManagementFileSystemTestsModule.OutsidePath;

    private IBlobProvider BlobProvider => GetRequiredService<IBlobProvider>();

    private ICurrentTenant CurrentTenant => GetRequiredService<ICurrentTenant>();

    [Theory]
    [InlineData("..\\..\\..\\outside\\pwn.txt")]
    [InlineData("../../outside/pwn.txt")]
    [InlineData("..\\..\\..\\..\\..\\outside/pwn.txt")]
    [InlineData("folder/../../outside/pwn.txt")]
    [InlineData("folder\\..\\..\\..\\outside\\pwn.txt")]
    // 类 Unix 上同样必须拒绝: 分隔符与非法字符按跨平台并集判断
    [InlineData("..\\..\\..\\etc\\passwd")]
    [InlineData("C:\\Windows\\Temp\\pwn.txt")]
    [InlineData("..")]
    [InlineData("folder/..")]
    // NTFS 备用数据流 / Windows 非法字符
    [InlineData("pwn.txt:evil")]
    [InlineData("pwn*star.txt")]
    [InlineData("pwn?question.txt")]
    [InlineData("pwn\"quote.txt")]
    [InlineData("pwn|pipe.txt")]
    [InlineData("pwn<less.txt")]
    [InlineData("pwn>greater.txt")]
    // Windows 归一化碰撞与保留设备名
    [InlineData("victim.txt.")]
    [InlineData("victim.txt ")]
    [InlineData("NUL")]
    [InlineData("con.txt")]
    [InlineData("COM1")]
    [InlineData("lpt9.log")]
    [InlineData("\\\\127.0.0.1\\c$\\pwn.txt")]
    public async Task Should_Reject_Unsafe_Blob_Names_On_Upload(string blobName)
    {
        var exception = await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.UploadBlobAsync(ContainerName, blobName, new MemoryStream(new byte[] { 1, 2, 3 })));

        exception.Code.ShouldBe(BlobManagementErrorCodes.Blob.InvalidName);
        Directory.GetFileSystemEntries(OutsidePath).ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Reject_Absolute_Path_On_Upload()
    {
        var absolute = Path.Combine(OutsidePath, "pwn-absolute.txt");

        var exception = await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.UploadBlobAsync(ContainerName, absolute, new MemoryStream(new byte[] { 1 })));

        exception.Code.ShouldBe(BlobManagementErrorCodes.Blob.InvalidName);
        File.Exists(absolute).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Reject_Traversal_On_Download()
    {
        var secret = Path.Combine(OutsidePath, "secret.txt");
        await File.WriteAllTextAsync(secret, "TOP-SECRET");

        var exception = await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.DownloadBlobAsync(ContainerName, "..\\..\\..\\outside\\secret.txt"));

        exception.Code.ShouldBe(BlobManagementErrorCodes.Blob.InvalidName);
    }

    [Fact]
    public async Task Should_Reject_Traversal_On_Delete()
    {
        var canary = Path.Combine(OutsidePath, "canary.txt");
        await File.WriteAllTextAsync(canary, "delete-me");

        var exception = await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.DeleteBlobAsync(ContainerName, "..\\..\\..\\outside\\canary.txt"));

        exception.Code.ShouldBe(BlobManagementErrorCodes.Blob.InvalidName);
        File.Exists(canary).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Reject_Traversal_On_Folder_Operations()
    {
        var emptyFolder = Path.Combine(OutsidePath, "empty-folder");
        Directory.CreateDirectory(emptyFolder);

        var createException = await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.CreateFolderAsync(ContainerName, "..\\..\\..\\outside\\new-folder"));
        createException.Code.ShouldBe(BlobManagementErrorCodes.Blob.InvalidName);

        var deleteException = await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.DeleteFolderAsync(ContainerName, "..\\..\\..\\outside\\empty-folder"));
        deleteException.Code.ShouldBe(BlobManagementErrorCodes.Blob.InvalidName);

        Directory.Exists(emptyFolder).ShouldBeTrue();
    }

    [Theory]
    // 逃逸: 容器名带 .. 试图离开 BasePath
    [InlineData("abp-c/../..")]
    [InlineData("..\\..\\evil")]
    [InlineData("../../evil")]
    // 容器别名: 结果仍在 BasePath 内, 会落到其它容器目录
    [InlineData("x/../abp-c")]
    [InlineData("x\\..\\abp-c")]
    [InlineData("users/../../abp-users")]
    // Windows 归一化碰撞
    [InlineData("abp-c.")]
    [InlineData("abp-c ")]
    [InlineData(".")]
    [InlineData("..")]
    public async Task Should_Reject_Unsafe_Container_Names(string containerName)
    {
        var exception = await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.CreateContainerAsync(containerName));

        exception.Code.ShouldBe(BlobManagementErrorCodes.Container.InvalidName);
    }

    [Fact]
    public async Task Should_Not_Allow_Container_Alias_To_Reach_Another_Container()
    {
        await BlobProvider.UploadBlobAsync(ContainerName, "original.txt", new MemoryStream(new byte[] { 1 }));
        var original = Path.Combine(BasePath, "host", ContainerName, "original.txt");
        File.Exists(original).ShouldBeTrue();

        var uploadException = await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.UploadBlobAsync("x/../abp-c", "aliased.txt", new MemoryStream(new byte[] { 2 })));
        uploadException.Code.ShouldBe(BlobManagementErrorCodes.Container.InvalidName);
        File.Exists(Path.Combine(BasePath, "host", ContainerName, "aliased.txt")).ShouldBeFalse();

        var downloadException = await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.DownloadBlobAsync("x/../abp-c", "original.txt"));
        downloadException.Code.ShouldBe(BlobManagementErrorCodes.Container.InvalidName);
    }

    [Theory]
    [InlineData("abp-c:evil", "abp-cevil")]
    [InlineData("abp-c*star", "abp-cstar")]
    [InlineData("abp-c?question", "abp-cquestion")]
    public async Task Should_Neutralize_Invalid_Characters_Of_Container_Names(string containerName, string expectedDirectoryName)
    {
        // ABP 的 FileSystemBlobNamingNormalizer 会剥掉非法字符
        await BlobProvider.CreateContainerAsync(containerName);

        Directory.Exists(Path.Combine(BasePath, "host", expectedDirectoryName)).ShouldBeTrue();
    }

    [Theory]
    [InlineData("public")]
    [InlineData("users")]
    [InlineData("my-container")]
    [InlineData("对象存储")]
    public async Task Should_Allow_Legitimate_Container_Names(string containerName)
    {
        await BlobProvider.CreateContainerAsync(containerName);

        Directory.Exists(Path.Combine(BasePath, "host", "abp-" + containerName)).ShouldBeTrue();
    }

    [Theory]
    [InlineData("file.txt")]
    [InlineData("folder/file.txt")]
    [InlineData("folder/sub/file.txt")]
    [InlineData("目录/子目录/文件.txt")]
    [InlineData("/folder/leading-slash.txt")]
    [InlineData("folder/with space.txt")]
    public async Task Should_Allow_Legitimate_Blob_Names(string blobName)
    {
        await BlobProvider.UploadBlobAsync(ContainerName, blobName, new MemoryStream(Encoding.UTF8.GetBytes("legit-content")));

        var expected = Path.Combine(
            BasePath, "host", ContainerName,
            blobName.RemovePreFix("/").Replace('\\', Path.DirectorySeparatorChar));
        File.Exists(expected).ShouldBeTrue();

        using (var stream = await BlobProvider.DownloadBlobAsync(ContainerName, blobName))
        {
            stream.ShouldNotBeNull();
            using var reader = new StreamReader(stream!);
            (await reader.ReadToEndAsync()).ShouldBe("legit-content");
        }

        await BlobProvider.DeleteBlobAsync(ContainerName, blobName);
        File.Exists(expected).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Keep_Nested_Folder_Operations_Working()
    {
        await BlobProvider.CreateFolderAsync(ContainerName, "folder/sub/empty");

        var folder = Path.Combine(BasePath, "host", ContainerName, "folder", "sub", "empty");
        Directory.Exists(folder).ShouldBeTrue();

        await File.WriteAllTextAsync(Path.Combine(folder, "file.txt"), "x");
        await Should.ThrowAsync<BusinessException>(async () =>
            await BlobProvider.DeleteFolderAsync(ContainerName, "folder/sub/empty"));

        File.Delete(Path.Combine(folder, "file.txt"));
        await BlobProvider.DeleteFolderAsync(ContainerName, "folder/sub/empty");
        Directory.Exists(folder).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Isolate_Host_And_Tenant_Directories()
    {
        var tenantId = Guid.NewGuid();

        await BlobProvider.UploadBlobAsync(ContainerName, "host.txt", new MemoryStream(new byte[] { 1 }));
        File.Exists(Path.Combine(BasePath, "host", ContainerName, "host.txt")).ShouldBeTrue();

        using (CurrentTenant.Change(tenantId))
        {
            await BlobProvider.UploadBlobAsync(ContainerName, "tenant.txt", new MemoryStream(new byte[] { 2 }));
            File.Exists(Path.Combine(BasePath, "tenants", tenantId.ToString("D"), ContainerName, "tenant.txt")).ShouldBeTrue();

            // 租户读取主机目录(租户目录之外的相对路径)会被拒绝
            await Should.ThrowAsync<BusinessException>(async () =>
                await BlobProvider.DownloadBlobAsync(ContainerName, "..\\..\\..\\host\\abp-c\\host.txt"));
        }
    }

    /// <summary>
    /// 路径包含性比较必须与文件系统大小写语义一致
    /// </summary>
    [Fact]
    public void Should_Use_File_System_Case_Semantics_For_Containment()
    {
        var expected = OperatingSystem.IsWindows() || OperatingSystem.IsMacOS()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        var property = typeof(LINGYUN.Abp.BlobManagement.FileSystem.FileSystemBlobProvider).GetProperty(
            "PathComparison",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        property.ShouldNotBeNull();
        property!.GetValue(null).ShouldBe(expected);
    }
}
