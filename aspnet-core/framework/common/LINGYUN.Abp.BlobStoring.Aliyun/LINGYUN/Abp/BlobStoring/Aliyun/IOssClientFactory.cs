using Aliyun.OSS;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public interface IOssClientFactory
    {
        /// <summary>
        /// 构建Oss客户端
        /// </summary>
        /// <returns></returns>
        Task<IOss> CreateAsync();
    }
}
