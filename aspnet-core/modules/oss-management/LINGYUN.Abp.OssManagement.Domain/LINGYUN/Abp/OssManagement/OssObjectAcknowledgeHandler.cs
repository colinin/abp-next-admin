using System;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.OssManagement;

public class OssObjectAcknowledgeHandler : IDistributedEventHandler<OssObjectAcknowledgeEto>, ITransientDependency
{
    protected const string Bucket = "temp";

    private readonly IOssContainerFactory _containerFactory;

    public OssObjectAcknowledgeHandler(IOssContainerFactory containerFactory)
    {
        _containerFactory = containerFactory;
    }

    public async virtual Task HandleEventAsync(OssObjectAcknowledgeEto eventData)
    {
        var ossContainer = _containerFactory.Create();

        var tempPath = HttpUtility.UrlDecode(GetTempPath(eventData.TempPath));
        var tempObject = HttpUtility.UrlDecode(GetTempObject(eventData.TempPath));

        var ossObject = await ossContainer.GetObjectAsync(Bucket, tempObject, tempPath, createPathIsNotExists: true);

        using (ossObject.Content)
        {
            var createOssObjectRequest = new CreateOssObjectRequest(
                eventData.Bucket,
                eventData.Object,
                ossObject.Content,
                eventData.Path,
                eventData.ExpirationTime);

            await ossContainer.CreateObjectAsync(createOssObjectRequest);
        }
    }

    private static string GetTempPath(string tempPath)
    {
        // api/files/static/demo-tenant-id/temp/p/path/file.txt   =>  path
        if (tempPath.Contains(Bucket))
        {
            // api/files/static/demo-tenant-id/temp/p/path/file.txt   =>  p/path/file.txt
            var lastIndex = tempPath.LastIndexOf(Bucket);

            tempPath = tempPath.Substring(lastIndex + Bucket.Length);

            // p/path/file.txt => 6
            var pathCharIndex = tempPath.LastIndexOf("/");
            if (pathCharIndex >= 0)
            {
                // p/path/file.txt => 0, 6 => 
                tempPath = tempPath.Substring(0, pathCharIndex);
            }
        }

        // 对目录url进行处理
        var pathIndex = tempPath.LastIndexOf("p/");
        if (pathIndex >= 0)
        {
            tempPath = tempPath.Substring(pathIndex + 2);
        }

        // 尾部不是 / 符号则不是目录
        if (!tempPath.EndsWith("/"))
        {
            var pathCharIndex = tempPath.LastIndexOf("/");
            if (pathCharIndex >= 0)
            {
                // path/file.txt => 0, 4 => 
                tempPath = tempPath.Substring(0, pathCharIndex);
            }
        }

        return tempPath.RemovePreFix("/");
    }

    private static string GetTempObject(string tempPath)
    {
        // api/files/static/demo-tenant-id/temp/path/file.txt   =>  file.txt

        var path = GetTempPath(tempPath);

        if (tempPath.EndsWith(path))
        {
            var fileNameIndex = tempPath.LastIndexOf("/");
            if (fileNameIndex >= 0)
            {
                return tempPath.Substring(fileNameIndex + 1);
            }
        }

        var pathIndex = tempPath.LastIndexOf(path);
        if (pathIndex >= 0)
        {
            tempPath = tempPath.Substring(pathIndex + path.Length);
        }

        return tempPath.RemovePreFix("/");
    }
}
