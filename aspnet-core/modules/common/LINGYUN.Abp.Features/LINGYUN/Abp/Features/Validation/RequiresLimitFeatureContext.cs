using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Features.Validation
{
    public class RequiresLimitFeatureContext
    {
        /// <summary>
        /// 功能限制策略
        /// </summary>
        public LimitPolicy Policy { get; }
        /// <summary>
        /// 功能限制时长
        /// </summary>
        public int Limit { get; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string Feature { get; }

        private Lazy<IDictionary<LimitPolicy, Func<int, long>>> effectPolicysLazy;
        private IDictionary<LimitPolicy, Func<int, long>> effectPolicys => effectPolicysLazy.Value;
        public RequiresLimitFeatureContext(
            string feature,
            LimitPolicy policy = LimitPolicy.Month,
            int limit = 1)
        {
            Limit = limit;
            Policy = policy;
            Feature = feature;

            effectPolicysLazy = new Lazy<IDictionary<LimitPolicy, Func<int, long>>>(() => CreateFeatureLimitPolicy());
        }

        /// <summary>
        /// 获取生效时间戳
        /// </summary>
        /// <returns></returns>
        public long GetEffectTicks()
        {
            return effectPolicys[Policy](Limit);
        }

        protected IDictionary<LimitPolicy, Func<int, long>> CreateFeatureLimitPolicy()
        {
            return new Dictionary<LimitPolicy, Func<int, long>>()
            {
                { LimitPolicy.Days, (time) => { return (long)(DateTimeOffset.UtcNow.AddDays(time) - DateTimeOffset.UtcNow).TotalSeconds; } },
                { LimitPolicy.Hours, (time) => { return (long)(DateTimeOffset.UtcNow.AddHours(time) - DateTimeOffset.UtcNow).TotalSeconds; } },
                { LimitPolicy.Month, (time) => { return (long)(DateTimeOffset.UtcNow.AddMonths(time) - DateTimeOffset.UtcNow).TotalSeconds; } },
                { LimitPolicy.Weeks, (time) => { return (long)(DateTimeOffset.UtcNow.AddDays(time * 7) - DateTimeOffset.UtcNow).TotalSeconds; } },
                { LimitPolicy.Years, (time) => { return (long)(DateTimeOffset.UtcNow.AddYears(time) - DateTimeOffset.UtcNow).TotalSeconds; } }
            };
        }
    }
}
