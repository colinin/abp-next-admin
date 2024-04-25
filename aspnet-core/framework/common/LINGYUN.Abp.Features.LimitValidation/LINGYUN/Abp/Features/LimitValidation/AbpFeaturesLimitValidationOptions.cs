using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Features.LimitValidation
{
    public class AbpFeaturesLimitValidationOptions
    {
        public IDictionary<LimitPolicy, Func<DateTime, int, long>> EffectPolicys { get; }

        public AbpFeaturesLimitValidationOptions()
        {
            EffectPolicys = new Dictionary<LimitPolicy, Func<DateTime, int, long>>();
        }
        /// <summary>
        /// 变更功能限制策略时长计算方法
        /// </summary>
        /// <param name="policy">限制策略</param>
        /// <param name="func">自定义的计算方法</param>
        /// <remarks>
        /// 返回值一定要是秒钟刻度
        /// </remarks>
        public void MapEffectPolicy(LimitPolicy policy,[NotNull] Func<DateTime, int, long> func)
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
            MapEffectPolicy(LimitPolicy.Minute, (now, tick) =>
            {
                return (long)(now.AddMinutes(tick) - now).TotalSeconds;
            });

            MapEffectPolicy(LimitPolicy.Hours, (now, tick) =>
            {
                return (long)(now.AddHours(tick) - now).TotalSeconds;
            });

            MapEffectPolicy(LimitPolicy.Days, (now, tick) =>
            {
                // 按天计算应取当天
                return (long)(now.Date.AddDays(tick) - now).TotalSeconds;
            });

            MapEffectPolicy(LimitPolicy.Weeks,(now, tick) =>
            {
                // 按周计算应取当周
                var nowDate = now.Date;
                var dayOfWeek = (int)nowDate.DayOfWeek - 1;
                if (nowDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    dayOfWeek = 6;
                }
                var utcOnceDayOfWeek = nowDate.AddDays(-dayOfWeek);

                return (long)(utcOnceDayOfWeek.AddDays(tick * 7) - now).TotalSeconds;
            });

            MapEffectPolicy(LimitPolicy.Month, (now, tick) =>
            {
                // 按月计算应取当月
                var utcOnceDayOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, 0);

                return (long)(utcOnceDayOfMonth.AddMonths(tick) - now).TotalSeconds; 
            });

            MapEffectPolicy(LimitPolicy.Years, (now, tick) => 
            {
                // 按年计算应取当年
                var utcOnceDayOfYear = new DateTime(now.Year, 1, 1, 0, 0, 0, 0);

                return (long)(utcOnceDayOfYear.AddYears(tick) - now).TotalSeconds; 
            });
        }
    }
}
