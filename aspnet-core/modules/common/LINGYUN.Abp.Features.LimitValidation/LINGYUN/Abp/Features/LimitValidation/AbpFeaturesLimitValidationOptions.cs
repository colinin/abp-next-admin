using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Features.LimitValidation
{
    public class AbpFeaturesLimitValidationOptions
    {
        public IDictionary<LimitPolicy, Func<int, long>> EffectPolicys { get; }
        public AbpFeaturesLimitValidationOptions()
        {
            EffectPolicys = new Dictionary<LimitPolicy, Func<int, long>>();
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

            if (EffectPolicys.ContainsKey(policy))
            {
                EffectPolicys[policy] = func;
            }
            else
            {
                EffectPolicys.Add(policy, func);
            }
        }

        internal void MapDefaultEffectPolicys()
        {
            MapEffectPolicy(LimitPolicy.Minute, (time) =>
            {
                var utcNow = DateTime.UtcNow;
                return (long)(utcNow.AddMinutes(time) - utcNow).TotalSeconds;
            });

            MapEffectPolicy(LimitPolicy.Hours, (time) =>
            {
                var utcNow = DateTime.UtcNow;
                return (long)(utcNow.AddHours(time) - utcNow).TotalSeconds;
            });

            MapEffectPolicy(LimitPolicy.Days, (time) =>
            {
                // 按天计算应取当天
                return (long)(DateTime.UtcNow.Date.AddDays(time) - DateTime.UtcNow).TotalSeconds;
            });

            MapEffectPolicy(LimitPolicy.Weeks,(time) =>
            {
                // 按周计算应取当周
                var utcNow = DateTime.UtcNow.Date;
                var dayOfWeek = (int)utcNow.DayOfWeek - 1;
                if (utcNow.DayOfWeek == DayOfWeek.Sunday)
                {
                    dayOfWeek = 6;
                }
                var utcOnceDayOfWeek = utcNow.AddDays(-dayOfWeek);

                return (long)(utcOnceDayOfWeek.AddDays(time * 7) - DateTime.UtcNow).TotalSeconds;
            });

            MapEffectPolicy(LimitPolicy.Month, (time) =>
            {
                // 按月计算应取当月
                var utcNow = DateTime.UtcNow;
                var utcOnceDayOfMonth = new DateTime(utcNow.Year, utcNow.Month, 1, 0, 0, 0, 0);

                return (long)(utcOnceDayOfMonth.AddMonths(time) - utcNow).TotalSeconds; 
            });

            MapEffectPolicy(LimitPolicy.Years, (time) => 
            {
                // 按年计算应取当年
                var utcNow = DateTime.UtcNow;
                var utcOnceDayOfYear = new DateTime(utcNow.Year, 1, 1, 0, 0, 0, 0);

                return (long)(utcOnceDayOfYear.AddYears(time) - utcNow).TotalSeconds; 
            });
        }
    }
}
