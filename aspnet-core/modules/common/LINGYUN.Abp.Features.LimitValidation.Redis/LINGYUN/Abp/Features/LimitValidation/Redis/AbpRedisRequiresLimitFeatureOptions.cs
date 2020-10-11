using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace LINGYUN.Abp.Features.LimitValidation.Redis
{
    public class AbpRedisRequiresLimitFeatureOptions : IOptions<AbpRedisRequiresLimitFeatureOptions>
    {
        public string Configuration { get; set; }
        public string InstanceName { get; set; }
        public ConfigurationOptions ConfigurationOptions { get; set; }
        AbpRedisRequiresLimitFeatureOptions IOptions<AbpRedisRequiresLimitFeatureOptions>.Value
        {
            get { return this; }
        }
    }
}
