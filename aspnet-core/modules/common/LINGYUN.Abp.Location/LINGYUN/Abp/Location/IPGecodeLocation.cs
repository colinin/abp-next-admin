using System.Collections.Generic;

namespace LINGYUN.Abp.Location
{
    public class IPGecodeLocation
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 定位坐标
        /// </summary>
        public Location Location { get; set; } = new Location();
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// adcode
        /// </summary>
        public string AdCode { get; set; }

        public IDictionary<string, object> Additionals { get; } = new Dictionary<string, object>();

        public void AddAdditional(string key, object value)
        {
            Additionals.Add(key, value);
        }
    }
}
