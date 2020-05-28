using System.Collections.Generic;

namespace LINGYUN.Abp.Location
{
    /// <summary>
    /// 正地址
    /// </summary>
    public class GecodeLocation : Location
    {
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
        /// <summary>
        /// 附加信息
        /// </summary>
        public IDictionary<string, object> Additionals { get; }

        public GecodeLocation()
        {
            Additionals = new Dictionary<string, object>();
        }

        public void AddAdditional(string key, object value)
        {
            Additionals.Add(key, value);
        }
    }
}
