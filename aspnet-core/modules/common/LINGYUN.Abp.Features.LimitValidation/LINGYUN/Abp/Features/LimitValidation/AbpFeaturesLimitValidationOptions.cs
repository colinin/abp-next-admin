using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Features.LimitValidation
{
    public class AbpFeaturesLimitValidationOptions
    {
        public IDictionary<LimitPolicy, Func<int, long>> EffectPolicies { get; }
        public AbpFeaturesLimitValidationOptions()
        {
            EffectPolicies = new Dictionary<LimitPolicy, Func<int, long>>();
        }
        /// <summary>
        /// 变更功能限制策略时长计算方法
        /// </summary>
        /// <param name="policy">限制策略</param>
        /// <param name="func">自定义的计算方法</param>
        /// <remarks>
        /// 返回值一定要是秒钟刻度
        /// </remarks>
        public void MapEffectPolicy(LimitPolicy policy,[NotNull] Func<int, long> func)
        {
            Check.NotNull(func, nameof(func));

            if (EffectPolicies.ContainsKey(policy))
            {
                EffectPolicies[policy] = func;
            }
            else
            {
                EffectPolicies.Add(policy, func);
            }
        }

        internal void MapDefaultEffectPolicies()
        {
            MapEffectPolicy(LimitPolicy.Minute, (time) => { return (long)(DateTimeOffset.UtcNow.AddMinutes(time) - DateTimeOffset.UtcNow).TotalSeconds; });
            MapEffectPolicy(LimitPolicy.Hours, (time) => { return (long)(DateTimeOffset.UtcNow.AddHours(time) - DateTimeOffset.UtcNow).TotalSeconds; });
            MapEffectPolicy(LimitPolicy.Days, (time) => { return (long)(DateTimeOffset.UtcNow.AddDays(time) - DateTimeOffset.UtcNow).TotalSeconds; });
            MapEffectPolicy(LimitPolicy.Weeks, (time) => { return (long)(DateTimeOffset.UtcNow.AddDays(time * 7) - DateTimeOffset.UtcNow).TotalSeconds; });
            MapEffectPolicy(LimitPolicy.Month, (time) => { return (long)(DateTimeOffset.UtcNow.AddMonths(time) - DateTimeOffset.UtcNow).TotalSeconds; });
            MapEffectPolicy(LimitPolicy.Years, (time) => { return (long)(DateTimeOffset.UtcNow.AddYears(time) - DateTimeOffset.UtcNow).TotalSeconds; });
        }
    }
}
