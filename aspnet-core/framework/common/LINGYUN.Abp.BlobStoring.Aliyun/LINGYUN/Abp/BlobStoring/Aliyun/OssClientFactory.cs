using Aliyun.OSS;
using LINGYUN.Abp.Aliyun;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public class OssClientFactory : AliyunClientFactory<IOss>, IOssClientFactory, ITransientDependency
    {
        public OssClientFactory(
            ISettingProvider settingProvider,
            IDistributedCache<AliyunBasicSessionCredentialsCacheItem> cache) 
            : base(settingProvider, cache)
        {
        }
        /// <summary>
        /// 普通方式构建Oss客户端
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="accessKeyId"></param>
        /// <param name="accessKeySecret"></param>
        /// <returns></returns>
        protected override IOss GetClient(string regionId, string accessKeyId, string accessKeySecret)
        {
            return new OssClient(
                regionId,
                accessKeyId,
                accessKeySecret);
        }

        /// <summary>
        /// 通过用户安全令牌构建Oss客户端
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="accessKeyId"></param>
        /// <param name="accessKeySecret"></param>
        /// <param name="securityToken"></param>
        /// <returns></returns>
        protected override IOss GetSecurityTokenClient(string regionId, string accessKeyId, string accessKeySecret, string securityToken)
        {
            return new OssClient(
                regionId,
                accessKeyId,
                accessKeySecret,
                securityToken);
        }
    }
}
