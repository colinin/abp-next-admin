namespace LINGYUN.Abp.Features.LimitValidation
{
    public enum LimitPolicy : byte
    {
        /// <summary>
        /// 按分钟限制
        /// </summary>
        Minute = 0,
        /// <summary>
        /// 按小时限制
        /// </summary>
        Hours = 10,
        /// <summary>
        /// 按天限制
        /// </summary>
        Days = 20,
        /// <summary>
        /// 按周限制
        /// </summary>
        Weeks = 30,
        /// <summary>
        /// 按月限制
        /// </summary>
        Month = 40,
        /// <summary>
        /// 按年限制
        /// </summary>
        Years = 50
    }
}
