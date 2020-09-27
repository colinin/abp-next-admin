namespace LINGYUN.Abp.Features.Validation
{
    public class RequiresLimitFeatureContext
    {
        /// <summary>
        /// 功能限制策略
        /// </summary>
        public LimitPolicy Policy { get; }
        /// <summary>
        /// 限制时长
        /// </summary>
        public int Interval { get; }
        /// <summary>
        /// 功能限制时长
        /// </summary>
        public int Limit { get; }
        /// <summary>
        /// 功能限制次数名称
        /// </summary>
        public string LimitFeature { get; }

        public AbpFeaturesValidationOptions Options { get; }
        public RequiresLimitFeatureContext(
            string limitFeature,
            AbpFeaturesValidationOptions options,
            LimitPolicy policy = LimitPolicy.Month,
            int interval = 1,
            int limit = 1)
        {
            Limit = limit;
            Policy = policy;
            Interval = interval;
            LimitFeature = limitFeature;
            Options = options;
        }

        /// <summary>
        /// 获取生效时间跨度,单位:s
        /// </summary>
        /// <returns></returns>
        public long GetEffectTicks()
        {
            return Options.EffectPolicys[Policy](Interval);
        }
    }
}
