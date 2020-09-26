using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace LINGYUN.Abp.Features.Validation.Redis
{
    public class AbpRedisRequiresLimitFeatureOptions : IOptions<AbpRedisRequiresLimitFeatureOptions>
    {
        public string Configuration { get; set; }
        public string InstanceName { get; set; }
        public ConfigurationOptions ConfigurationOptions { get; set; }
        /// <summary>
        /// 失败重试次数
        /// default: 3
        /// </summary>
        public int FailedRetryCount { get; set; } = 3;
        /// <summary>
        /// 失败重试间隔 ms
        /// default: 1000
        /// </summary>
        public int FailedRetryInterval { get; set; } = 1000;
        AbpRedisRequiresLimitFeatureOptions IOptions<AbpRedisRequiresLimitFeatureOptions>.Value
        {
            get { return this; }
        }
    }
}
