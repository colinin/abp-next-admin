using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement.Integration;

public class OssObjectIntegrationService : OssManagementApplicationServiceBase, IOssObjectIntegrationService
{
    protected FileUploadMerger Merger { get; }
    protected IOssContainerFactory OssContainerFactory { get; }

    public OssObjectIntegrationService(
        FileUploadMerger merger,
        IOssContainerFactory ossContainerFactory)
    {
        Merger = merger;
        OssContainerFactory = ossContainerFactory;
    }

    public async virtual Task<OssObjectDto> CreateAsync(CreateOssObjectInput input)
    {
        // 内容为空时建立目录
        if (input.File == null || !input.File.ContentLength.HasValue)
        {
            var oss = CreateOssContainer();
            var request = new CreateOssObjectRequest(
                HttpUtility.UrlDecode(input.Bucket),
                HttpUtility.UrlDecode(input.FileName),
                Stream.Null,
                HttpUtility.UrlDecode(input.Path));
            var ossObject = await oss.CreateObjectAsync(request);

            return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
        }
        else
        {
            var ossObject = await Merger.MergeAsync(input);

            return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
        }
    }

    public async virtual Task DeleteAsync(GetOssObjectInput input)
    {
        var oss = CreateOssContainer();

        await oss.DeleteObjectAsync(input.Bucket, input.Object, input.Path);
    }

    public async virtual Task<GetOssObjectExistsResult> ExistsAsync(GetOssObjectInput input)
    {
        try
        {
            var oss = CreateOssContainer();

            var ossObject = await oss.GetObjectAsync(input.Bucket, input.Object, input.Path, input.MD5);

            return new GetOssObjectExistsResult
            {
                IsExists = ossObject != null
            };
        }
        catch (Exception)
        {
            return new GetOssObjectExistsResult
            {
                IsExists = false,
            };
        }
    }

    public async virtual Task<IRemoteStreamContent> GetAsync(GetOssObjectInput input)
    {
        try
        {
            var oss = CreateOssContainer();

            var ossObject = await oss.GetObjectAsync(input.Bucket, input.Object, input.Path, input.MD5);

            return new RemoteStreamContent(ossObject.Content, ossObject.Name);
        }
        catch (Exception)
        {
            return new RemoteStreamContent(Stream.Null);
        }
    }

    protected virtual IOssContainer CreateOssContainer()
    {
        return OssContainerFactory.Create();
    }
}
