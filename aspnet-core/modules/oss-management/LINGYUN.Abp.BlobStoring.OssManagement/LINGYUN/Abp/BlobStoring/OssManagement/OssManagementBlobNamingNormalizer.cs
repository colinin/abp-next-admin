using System;
using System.Text;
using System.Web;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.OssManagement
{
    public class OssManagementBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
    {
        public string NormalizeBlobName(string blobName)
        {
            // 路径需要URL编码
            return HttpUtility.UrlEncode(blobName, Encoding.UTF8);
        }

        public string NormalizeContainerName(string containerName)
        {
            // 取消反斜杠开头
            return containerName.EnsureStartsWith('/');
        }
    }
}
