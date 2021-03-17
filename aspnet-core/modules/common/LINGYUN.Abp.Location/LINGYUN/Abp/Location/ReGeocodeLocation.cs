using System.Collections.Generic;

namespace LINGYUN.Abp.Location
{
    /// <summary>
    /// 逆地址
    /// </summary>
    public class ReGeocodeLocation
    {
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 格式化的地址描述
        /// </summary>
        public string FormattedAddress { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// adcode
        /// </summary>
        public string AdCode { get; set; }
        /// <summary>
        /// 乡镇
        /// </summary>
        public string Town { get; set; }
        /// <summary>
        /// 门牌号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Poi信息列表
        /// </summary>
        public IEnumerable<Poi> Pois { get; set; }
        /// <summary>
        /// 道路信息列表
        /// </summary>
        public IEnumerable<Road> Roads { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public IDictionary<string, object> Additionals { get; }

        public ReGeocodeLocation()
        {
            Pois = new Poi[0];
            Roads = new Road[0];
            Additionals = new Dictionary<string, object>();
        }

        public void AddAdditional(string key, object value)
        {
            Additionals.Add(key, value);
        }
    }
}
