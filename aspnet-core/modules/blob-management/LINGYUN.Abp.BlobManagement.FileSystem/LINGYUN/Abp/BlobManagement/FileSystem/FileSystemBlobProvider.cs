using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.IO;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement.FileSystem;

public class FileSystemBlobProvider : IBlobProvider
{
    public const string ProviderName = "FileSystem";
    public string Name => ProviderName;

    protected ICurrentTenant CurrentTenant { get; }
    protected FileSystemBlobNamingNormalizer BlobNamingNormalizer { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

    public FileSystemBlobProvider(
        ICurrentTenant currentTenant,
        FileSystemBlobNamingNormalizer blobNamingNormalizer,
        IBlobContainerConfigurationProvider configurationProvider)
    {
        CurrentTenant = currentTenant;
        BlobNamingNormalizer = blobNamingNormalizer;
        ConfigurationProvider = configurationProvider;
    }

    public virtual Task CreateContainerAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var filePath = CalculateBlobName(NormalizeContainerName(name));

        DirectoryHelper.CreateIfNotExists(filePath);

        return Task.CompletedTask;
    }

    public virtual Task DeleteContainerAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var filePath = CalculateBlobName(NormalizeContainerName(name));

        DirectoryHelper.DeleteIfExists(filePath);

        return Task.CompletedTask;
    }

    public virtual Task DeleteBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var fileName = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        FileHelper.DeleteIfExists(fileName);

        return Task.CompletedTask;
    }

    public virtual Task DeleteFolderAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var filePath = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        var children = Directory.GetFileSystemEntries(filePath);
        if (children.Length > 0)
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Blob.DeleteWithNotEmpty,
                "The current directory is not empty and cannot be deleted!");
        }

        DirectoryHelper.DeleteIfExists(filePath);

        return Task.CompletedTask;
    }

    public virtual Task<Stream?> DownloadBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var fileName = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        if (!File.Exists(fileName))
        {
            return Task.FromResult<Stream?>(null);
        }

        return Task.FromResult<Stream?>(File.OpenRead(fileName));
    }

    public virtual Task<string?> GeneratePresignedUrlAsync(
        string containerName,
        string blobName,
        TimeSpan expiration,
        bool isAttachmentContent = true,
        CancellationToken cancellationToken = default)
    {
        // 本地文件系统无需生成下载链接
        return Task.FromResult<string?>(null);
    }

    public virtual Task CreateFolderAsync(
        string containerName, 
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var filePath = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        DirectoryHelper.CreateIfNotExists(filePath);

        return Task.CompletedTask;
    }

    public async virtual Task UploadBlobAsync(
        string containerName, 
        string blobName, 
        Stream content,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        var fileName = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(fileName)!);

        using var fileStream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);

        await content.CopyToAsync(fileStream, cancellationToken);

        await fileStream.FlushAsync(cancellationToken);
    }

    protected virtual FileSystemBlobProviderConfiguration GetBlobConfiguration()
    {
        var configuration = ConfigurationProvider.Get<BlobManagementContainer>();
        var blobConfiguration = configuration.GetFileSystemConfiguration();
        return blobConfiguration;
    }

    protected virtual string NormalizeContainerName(string containerName)
    {
        if (!containerName.StartsWith("abp-"))
        {
            containerName = "abp-" + containerName;
        }
        return BlobNamingNormalizer.NormalizeContainerName(containerName);
    }

    /// <summary>
    /// 路径比较方式
    /// </summary>
    /// <remarks>
    /// Windows 与 macOS(APFS/HFS+ 默认)大小写不敏感, Linux 等类 Unix 文件系统大小写敏感:
    /// 在大小写敏感的文件系统上使用忽略大小写比较, 会放过大小写变体的兄弟目录
    /// </remarks>
    protected static StringComparison PathComparison =>
        OperatingSystem.IsWindows() || OperatingSystem.IsMacOS()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

    /// <summary>
    /// 名称中不允许出现的字符
    /// </summary>
    /// <remarks>
    /// 显式使用各平台非法字符的并集(而非 <see cref="Path.GetInvalidFileNameChars"/>):
    /// 类 Unix 文件名允许 ':' 等字符, 但同一份数据可能被 Windows 宿主、SMB 共享、
    /// 或其它 blob provider 复用, 因此统一按可移植规则拒绝
    /// </remarks>
    protected static readonly char[] InvalidNameCharacters =
    [
        '\0', '/', '\\', ':', '*', '?', '"', '<', '>', '|',
    ];

    protected virtual string CalculateBlobName(string bucketName, string blobName = "")
    {
        var blobConfiguration = GetBlobConfiguration();
        var basePath = Path.GetFullPath(blobConfiguration.BasePath);

        // 当前租户的存储根目录: 主机为 host, 租户为 tenants/{tenantId}
        string tenantPath;
        if (CurrentTenant.Id == null)
        {
            tenantPath = Path.Combine(basePath, "host");
        }
        else
        {
            tenantPath = Path.Combine(basePath, "tenants", CurrentTenant.Id.Value.ToString("D"));
        }
        DirectoryHelper.CreateIfNotExists(tenantPath);

        var blobPath = tenantPath;

        if (blobConfiguration.AppendContainerNameToBasePath &&
            !bucketName.IsNullOrWhiteSpace())
        {
            CheckContainerName(bucketName);

            blobPath = Path.Combine(blobPath, bucketName);
        }
        if (!blobName.IsNullOrWhiteSpace())
        {
            // 模块存储的全名可能以 / 开头(如 /abp-container/folder/file.txt)
            var name = blobName.RemovePreFix("/");
            CheckBlobName(name);

            blobPath = Path.Combine(blobPath, name);
        }

        // 关键: 解析 .. / 绝对路径 / 分隔符变体之后, 结果必须仍在当前租户目录之内
        // (Path.GetFullPath 会消除 .., 因此绝对路径注入同样会被拦截)
        var fullPath = Path.GetFullPath(blobPath);
        var tenantRoot = tenantPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        if (!fullPath.StartsWith(tenantRoot, PathComparison))
        {
            throw BlobManagementNameValidator.CreateInvalidNameException(
                blobName,
                BlobManagementErrorCodes.Blob.InvalidName);
        }

        return fullPath;
    }

    /// <summary>
    /// 校验容器名称
    /// </summary>
    /// <remarks>
    /// 容器名必须是单一目录名: 含分隔符的名称不会逃出租户目录(containment 拦截不到),
    /// 但会解析成同一租户内的其它容器目录(容器别名), 从而绕过容器隔离。
    /// </remarks>
    protected virtual void CheckContainerName(string containerName)
    {
        if (containerName.IsNullOrWhiteSpace())
        {
            return;
        }

        BlobManagementNameValidator.CheckName(containerName, BlobManagementErrorCodes.Container.InvalidName);
    }

    /// <summary>
    /// 校验文件/目录名称(路径)
    /// </summary>
    /// <remarks>
    /// 规则与实体一致; 这只是纵深防御, 真正的安全边界是 <see cref="CalculateBlobName"/> 的 containment 校验
    /// (名称校验覆盖不到符号链接/联接点)。
    /// </remarks>
    protected virtual void CheckBlobName(string blobName)
    {
        if (blobName.IsNullOrWhiteSpace())
        {
            return;
        }

        BlobManagementNameValidator.CheckPath(blobName, BlobManagementErrorCodes.Blob.InvalidName);
    }
}
