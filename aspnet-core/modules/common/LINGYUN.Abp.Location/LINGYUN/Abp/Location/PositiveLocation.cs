namespace LINGYUN.Abp.Location
{
    /// <summary>
    /// 正地址
    /// </summary>
    public class PositiveLocation : Location
    {
        /// <summary>
        /// 附加信息
        /// </summary>
        public int Precise { get; set; }
        /// <summary>
        /// 绝对精度
        /// </summary>
        public int Confidence { get; set; }
        /// <summary>
        /// 理解程度
        /// 分值范围0-100
        /// 分值越大，服务对地址理解程度越高
        /// </summary>
        public int Pomprehension { get; set; }
        /// <summary>
        /// 能精确理解的地址类型
        /// </summary>
        public string Level { get; set; }
    }
}
