using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth;
using Aliyun.Acs.Core.Profile;
using LINGYUN.Abp.Aliyun.Features;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Aliyun
{
    [RequiresFeature(AliyunFeatureNames.Enable)]
    public class AcsClientFactory : AliyunClientFactory<IAcsClient>, IAcsClientFactory, ITransientDependency
    {
        public AcsClientFactory(
            ISettingProvider settingProvider,
            IDistributedCache<AliyunBasicSessionCredentialsCacheItem> cache)
            : base(settingProvider, cache)
        {
        }

        protected override IAcsClient GetClient(string regionId, string accessKeyId, string accessKeySecret)
        {
            return new DefaultAcsClient(
                DefaultProfile.GetProfile(regionId, accessKeyId, accessKeySecret));
        }

        protected override IAcsClient GetSecurityTokenClient(string regionId, string accessKeyId, string accessKeySecret, string securityToken)
        {
            var profile = DefaultProfile.GetProfile(regionId);
            var credentials = new BasicSessionCredentials(accessKeyId, accessKeySecret, securityToken);
            return new DefaultAcsClient(profile, credentials);
        }
    }
}
