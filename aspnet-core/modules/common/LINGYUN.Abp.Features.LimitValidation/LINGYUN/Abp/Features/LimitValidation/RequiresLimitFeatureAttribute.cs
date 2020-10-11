using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.Features.LimitValidation
{
    /// <summary>
    /// 单个功能的调用量限制
    /// </summary>
    /// <remarks>
    /// 需要对于限制时长和限制上限功能区分,以便于更细粒度的限制
    /// </remarks>
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
        /// 限制上限名称
        /// </summary>
        public string LimitFeature { get; }
        /// <summary>
        /// 默认限制时长
        /// </summary>
        public int DefaultInterval { get; }
        /// <summary>
        /// 限制时长名称
        /// </summary>
        public string IntervalFeature { get; }

        public RequiresLimitFeatureAttribute(
            [NotNull] string limitFeature,
            [NotNull] string intervalFeature,
            LimitPolicy policy = LimitPolicy.Month,
            int defaultLimit = 1,
            int defaultInterval = 1)
        {
            Check.NotNullOrWhiteSpace(limitFeature, nameof(limitFeature));
            Check.NotNullOrWhiteSpace(intervalFeature, nameof(intervalFeature));

            Policy = policy;
            LimitFeature = limitFeature;
            DefaultLimit = defaultLimit;
            IntervalFeature = intervalFeature;
            DefaultInterval = defaultInterval;
        }
    }
}
