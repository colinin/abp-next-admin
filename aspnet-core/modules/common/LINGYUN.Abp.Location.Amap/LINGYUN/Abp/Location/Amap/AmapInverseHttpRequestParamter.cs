namespace LINGYUN.Abp.Location.Amap
{
    public class AmapInverseHttpRequestParamter
    {
        /// <summary>
        /// 经纬度坐标
        /// 
        /// 传入内容规则：经度在前，纬度在后，经纬度间以“,”分割，经纬度小数点后不要超过 6 位。
        /// 如果需要解析多个经纬度的话，请用"|"进行间隔，并且将 batch 参数设置为 true，最多支持传入 20 对坐标点。
        /// 每对点坐标之间用"|"分割。
        /// </summary>
        public Location[] Locations { get; set; } = new Location[0];
        /// <summary>
        /// 返回附近POI类型
        /// 以下内容需要 extensions 参数为 all 时才生效。
        /// 逆地理编码在进行坐标解析之后不仅可以返回地址描述，也可以返回经纬度附近符合限定要求的POI内容（在 extensions 字段值为 all 时才会返回POI内容）。
        /// 设置 POI 类型参数相当于为上述操作限定要求。
        /// 
        /// 参数仅支持传入POI TYPECODE，可以传入多个POI TYPECODE，相互之间用“|”分隔。
        /// 该参数在 batch 取值为 true 时不生效。获取 POI TYPECODE
        /// </summary>
        public string PoiType { get; set; }
        /// <summary>
        /// 搜索半径
        /// radius取值范围在0~3000，默认是1000。单位：米
        /// </summary>
        public short Radius { get; set; } = 1000;
        /// <summary>
        /// 返回结果控制
        /// extensions 参数默认取值是 base，也就是返回基本地址信息；
        /// extensions 参数取值为 all 时会返回基本地址信息、附近 POI 内容、道路信息以及道路交叉口信息。
        /// </summary>
        public string Extensions { get; set; } = "base";
        /// <summary>
        /// 批量查询控制
        /// batch 参数设置为 true 时进行批量查询操作，最多支持 20 个经纬度点进行批量地址查询操作。
        /// batch 参数设置为 false 时进行单点查询，此时即使传入多个经纬度也只返回第一个经纬度的地址解析查询结果。
        /// </summary>
        public bool Batch { get; set; } = false;
        /// <summary>
        /// 道路等级
        /// 
        /// 以下内容需要 extensions 参数为 all 时才生效。
        /// 可选值：0，1
        /// 当roadlevel=0时，显示所有道路
        /// 当roadlevel = 1时，过滤非主干道路，仅输出主干道路数据
        /// </summary>
        public sbyte? RoadLevel { get; set; }
        /// <summary>
        /// 数字签名
        /// </summary>
        public string Sig { get; set; }
        /// <summary>
        /// 返回数据格式类型
        /// 可选输入内容包括：JSON，XML。设置 JSON 返回结果数据将会以JSON结构构成；
        /// 如果设置 XML 返回结果数据将以 XML 结构构成。
        /// </summary>
        public string Output { get; set; } = "JSON";
        /// <summary>
        /// 是否优化POI返回顺序
        /// 以下内容需要 extensions 参数为 all 时才生效。
        /// homeorcorp 参数的设置可以影响召回 POI 内容的排序策略，目前提供三个可选参数：
        /// 
        /// 0：不对召回的排序策略进行干扰。
        /// 
        /// 1：综合大数据分析将居家相关的 POI 内容优先返回，即优化返回结果中 pois 字段的poi顺序。
        /// 
        /// 2：综合大数据分析将公司相关的 POI 内容优先返回，即优化返回结果中 pois 字段的poi顺序。
        /// </summary>
        public sbyte HomeOrCorp { get; set; } = 0;
    }
}
