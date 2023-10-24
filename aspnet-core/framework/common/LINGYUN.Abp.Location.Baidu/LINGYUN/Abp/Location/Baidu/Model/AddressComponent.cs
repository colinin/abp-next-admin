using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Location.Baidu.Model
{
    public class AddressComponent
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 国家国家编码
        /// </summary>
        [JsonProperty("country_code")]
        [JsonPropertyName("country_code")]
        public int CountryCode { get; set; }
        /// <summary>
        /// 国家英文缩写（三位）
        /// </summary>
        [JsonProperty("country_code_iso")]
        [JsonPropertyName("country_code_iso")]
        public string CountryCodeIso { get; set; }
        /// <summary>
        /// 国家英文缩写（两位）
        /// </summary>
        [JsonProperty("country_code_iso2")]
        [JsonPropertyName("country_code_iso2")]
        public string CountryCodeIso2 { get; set; }
        /// <summary>
        /// 省名
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市名
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 城市所在级别（仅国外有参考意义。
        /// 国外行政区划与中国有差异，城市对应的层级不一定为『city』。
        /// country、province、city、district、town分别对应0-4级，若city_level=3，则district层级为该国家的city层级）
        /// </summary>
        [JsonProperty("city_level")]
        [JsonPropertyName("city_level")]
        public int CityLevel { get; set; }
        /// <summary>
        /// 区县名
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 乡镇名
        /// </summary>
        public string Town { get; set; }
        /// <summary>
        /// 乡镇id
        /// </summary>
        [JsonProperty("town_code")]
        [JsonPropertyName("town_code")]
        public string TownCode { get; set; }
        /// <summary>
        /// 街道名（行政区划中的街道层级）
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 街道门牌号
        /// </summary>
        [JsonProperty("street_number")]
        [JsonPropertyName("street_number")]
        public string StreetNumber { get; set; }
        /// <summary>
        /// 行政区划代码
        /// </summary>
        public string AdCode { get; set; }
        /// <summary>
        /// 相对当前坐标点的方向，当有门牌号的时候返回数据
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 相对当前坐标点的距离，当有门牌号的时候返回数据
        /// </summary>
        public string Distance { get; set; }
    }
}
