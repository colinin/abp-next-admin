namespace LINGYUN.Abp.Features.Validation
{
    public enum LimitPolicy
    {
        /// <summary>
        /// 按小时限制
        /// </summary>
        Hours = 0,
        /// <summary>
        /// 按天限制
        /// </summary>
        Days = 1,
        /// <summary>
        /// 按周限制
        /// </summary>
        Weeks = 2,
        /// <summary>
        /// 按月限制
        /// </summary>
        Month = 3,
        /// <summary>
        /// 按年限制
        /// </summary>
        Years = 4
    }
}
