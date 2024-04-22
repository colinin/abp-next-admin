using System;

namespace LINGYUN.Abp.Location.Baidu
{
    public class BaiduLocationOptions
    {
        /// <summary>
        /// 用户申请注册的key
        /// </summary>
        public string AccessKey { get; set; }
        /// <summary>
        /// 用户申请注册的AccessSecret
        /// </summary>
        public string AccessSecret { get; set; }
        /// <summary>
        /// 坐标的类型，目前支持的坐标类型包括：
        /// bd09ll（百度经纬度坐标）
        /// bd09mc（百度米制坐标）
        /// gcj02ll（国测局经纬度坐标，仅限中国）
        /// wgs84ll（ GPS经纬度）
        /// </summary>
        public string CoordType { get; set; } = "bd09ll";
        /// <summary>
        /// 可选参数，添加后返回国测局经纬度坐标或百度米制
        /// gcj02ll（国测局坐标）
        /// bd09mc（百度墨卡托坐标）
        /// </summary>
        public string ReturnCoordType { get; set; } = "bd09ll";
        /// <summary>
        /// 计算sn
        /// </summary>
        public bool CaculateAKSN => !AccessSecret.IsNullOrWhiteSpace();
        /// <summary>
        /// extensions_poi=0，不召回pois数据。
        /// extensions_poi=1，返回pois数据，
        /// 默认显示周边1000米内的poi。
        /// </summary>
        public string ExtensionsPoi { get; set; } = "1";
        /// <summary>
        /// 当取值为true时，召回坐标周围最近的3条道路数据。
        /// 区别于行政区划中的street参数（street参数为行政区划中的街道，和普通道路不对应）
        /// </summary>
        public bool ExtensionsRoad { get; set; } = false;
        /// <summary>
        /// 当取值为true时，行政区划返回乡镇级数据（仅国内召回乡镇数据）。
        /// 默认不访问。
        /// </summary>
        public bool ExtensionsTown { get; set; } = false;
        /// <summary>
        /// 指定召回的新政区划语言类型。
        /// el gu en vi ca it iw sv eu ar cs gl id es en-GB ru sr nl pt tr tl lv en-AU lt th ro 
        /// fil ta fr bg hr bn de hu fa hi pt-BR fi da ja te pt-PT ml ko kn sk zh-CN pl uk sl mr 
        /// local
        /// </summary>
        public string Language { get; set; } = "zh-CN";
        /// <summary>
        /// 输出格式为json或者xml
        /// </summary>
        public string Output { get; set; } = "json";
        /// <summary>
        /// 展示错误给客户端
        /// </summary>
        public bool VisableErrorToClient { get; set; } = false;
    }
}
