using System;

namespace LINGYUN.Abp.Features.Validation
{
    /// <summary>
    /// 单个功能的调用量限制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RequiresLimitFeatureAttribute : Attribute
    {
        /// <summary>
        /// 功能限制策略
        /// </summary>
        public LimitPolicy Policy { get; }
        /// <summary>
        /// 默认限制时长
        /// </summary>
        public int DefaultLimit { get; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string Feature { get; }

        public RequiresLimitFeatureAttribute(
            string feature,
            LimitPolicy policy = LimitPolicy.Month,
            int defaultLimit = 1)
        {
            DefaultLimit = defaultLimit;
            Policy = policy;
            Feature = feature;
        }
    }
}
