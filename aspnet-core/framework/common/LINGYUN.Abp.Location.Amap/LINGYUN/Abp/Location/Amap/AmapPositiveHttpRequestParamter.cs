using JetBrains.Annotations;

namespace LINGYUN.Abp.Location.Amap
{
    /// <summary>
    /// 高德地图正地址解析请求参数
    /// </summary>
    public class AmapPositiveHttpRequestParamter
    {
        /// <summary>
        /// 结构化的地址
        /// 规则遵循：国家、省份、城市、区县、城镇、乡村、街道、门牌号码、屋邨、大厦，如：北京市朝阳区阜通东大街6号。
        /// 如果需要解析多个地址的话，请用"|"进行间隔，并且将 batch 参数设置为 true，最多支持 10 个地址进进行"|"分割形式的请求
        /// </summary>
        [NotNull]
        public string Address { get; set; }
        /// <summary>
        /// 城市
        /// 可选输入内容包括：指定城市的中文（如北京）、指定城市的中文全拼（beijing）、citycode（010）、adcode（110000），不支持县级市。
        /// 当指定城市查询内容为空时，会进行全国范围内的地址转换检索。
        /// </summary>
        [CanBeNull]
        public string City { get; set; }
        /// <summary>
        /// 批量查询控制
        /// batch 参数设置为 true 时进行批量查询操作，最多支持 10 个地址进行批量查询。
        /// batch 参数设置为 false 时进行单点查询，此时即使传入多个地址也只返回第一个地址的解析查询结果
        /// </summary>
        [CanBeNull]
        public bool Batch { get; set; } = false;
        /// <summary>
        /// 数字签名
        /// </summary>
        [CanBeNull]

        public string Sig { get; set; }
        /// <summary>
        /// 返回数据格式类型
        /// 可选输入内容包括：JSON，XML。设置 JSON 返回结果数据将会以JSON结构构成；
        /// 如果设置 XML 返回结果数据将以 XML 结构构成。
        /// </summary>
        [CanBeNull]
        public string Output { get; set; } = "JSON";
    }
}
