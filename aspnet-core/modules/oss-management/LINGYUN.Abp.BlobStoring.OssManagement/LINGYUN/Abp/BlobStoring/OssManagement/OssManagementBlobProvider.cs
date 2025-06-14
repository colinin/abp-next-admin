using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.Integration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.OssManagement;

public class OssManagementBlobProvider : BlobProviderBase, ITransientDependency
{
    public ILogger<OssManagementBlobProvider> Logger { protected get; set; }

    private readonly IOssObjectIntegrationService _ossObjectAppService;
    public OssManagementBlobProvider(
        IOssObjectIntegrationService ossObjectAppService)
    {
        _ossObjectAppService = ossObjectAppService;

        Logger = NullLogger<OssManagementBlobProvider>.Instance;
    }

    public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        await _ossObjectAppService.DeleteAsync(new GetOssObjectInput
        {
            Bucket = args.ContainerName,
            Path = GetOssPath(args),
            Object = GetOssName(args),
        });
        return true;
    }

    public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var result = await _ossObjectAppService.ExistsAsync(new GetOssObjectInput
        {
            Bucket = args.ContainerName,
            Path = GetOssPath(args),
            Object = GetOssName(args),
        });
        return result.IsExists;
    }

    public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var content = await _ossObjectAppService.GetAsync(new GetOssObjectInput
        {
            Bucket = args.ContainerName,
            Path = GetOssPath(args),
            Object = GetOssName(args),
        });

        return content?.GetStream();
    }

    public override async Task SaveAsync(BlobProviderSaveArgs args)
    {
        await _ossObjectAppService.CreateAsync(new CreateOssObjectInput
        {
            Bucket = args.ContainerName,
            Overwrite = args.OverrideExisting,
            Path = GetOssPath(args),
            FileName = GetOssName(args),
            File = new RemoteStreamContent(args.BlobStream)
        });
    }

    protected virtual string GetOssPath(BlobProviderArgs args)
    {
        // path1/path2/path3/path3/path5/file.txt   =>  path1/path2/path3/path3/path5/
        if (args.BlobName.Contains("/"))
        {
            var lastIndex = args.BlobName.LastIndexOf('/');
            return args.BlobName.Substring(0, lastIndex);
        }

        return args.BlobName.EnsureEndsWith('/');
    }

    protected virtual string GetOssName(BlobProviderArgs args)
    {
        // path1/path2/path3/path3/path5/file.txt   =>  file.txt
        if (args.BlobName.Contains("/"))
        {
            var lastIndex = args.BlobName.LastIndexOf('/');

            // TODO: 用户传递以 / 为结尾符的文件名,让系统抛出异常?
            return args.BlobName.Substring(lastIndex + 1);
        }

        return args.BlobName;
    }
}
