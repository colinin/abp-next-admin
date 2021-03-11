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
        Task<IOss> CreateAsync<TContainer>();
        /// <summary>
        /// 通过配置信息构建Oss客户端调用
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<IOss> CreateAsync(AliyunBlobProviderConfiguration configuration);
    }
}
