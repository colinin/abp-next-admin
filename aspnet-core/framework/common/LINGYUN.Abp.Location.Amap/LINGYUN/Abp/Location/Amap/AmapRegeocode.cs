using Newtonsoft.Json;
using System.Collections.Generic;

namespace LINGYUN.Abp.Location.Amap
{
    public class AmapRegeocode
    {
        /// <summary>
        /// 结构化地址信息
        /// 省份＋城市＋区县＋城镇＋乡村＋街道＋门牌号码
        /// </summary>
        [JsonProperty("formatted_address")]
        public string Address { get; set; }
        /// <summary>
        /// 地址元素列表
        /// </summary>
        public AddressComponent AddressComponent { get; set; }
        /// <summary>
        /// 道路信息列表
        /// 请求参数 extensions 为 all 时返回
        /// </summary>
        public Road[] Roads { get; set; }
        /// <summary>
        /// 道路交叉口列表
        /// 请求参数 extensions 为 all 时返回
        /// </summary>
        public Roadinter[] Roadinters { get; set; }
        /// <summary>
        /// poi信息列表
        /// 请求参数 extensions 为 all 时返回
        /// </summary>
        public Poi[] Pois { get; set; }
        /// <summary>
        /// aoi信息列表
        /// 请求参数 extensions 为 all 时返回
        /// </summary>
        public Aoi[] Aois { get; set; }
        public AmapRegeocode()
        {
            AddressComponent = new AddressComponent();
            Roads = new Road[0];
            Roadinters = new Roadinter[0];
            Pois = new Poi[0];
            Aois = new Aoi[0];
        }
    }

    public class AddressComponent
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 坐标点所在省名称
        /// 例如：北京市
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 坐标点所在城市名称
        /// 请注意：当城市是省直辖县时返回为空，以及城市为北京、上海、天津、重庆四个直辖市时，该字段返回为空
        /// </summary>
        public string[] City { get; set; }

        /// <summary>
        /// 城市编码
        /// 例如：010
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// 坐标点所在区
        /// 例如：海淀区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 行政区编码
        /// 例如：110108
        /// </summary>
        public string AdCode { get; set; }

        /// <summary>
        /// 坐标点所在乡镇/街道（此街道为社区街道，不是道路信息）
        /// 例如：燕园街道
        /// </summary>
        public string[] TownShip { get; set; }

        /// <summary>
        /// 乡镇街道编码
        /// 例如：110101001000
        /// </summary>
        public string TownCode { get; set; }
        /// <summary>
        /// 社区信息列表
        /// </summary>
        public NeighBorHood NeighBorHood { get; set; }
        /// <summary>
        /// 楼信息列表
        /// </summary>
        public Building Building { get; set; }
        /// <summary>
        /// 门牌信息列表
        /// </summary>
        public StreetNumber StreetNumber { get; set; }
        /// <summary>
        /// 所属海域信息
        /// 例如：渤海
        /// </summary>
        public string SeaArea { get; set; }
        /// <summary>
        /// 经纬度所属商圈列表
        /// </summary>
        public List<BusinessArea> BusinessAreas { get; set; }
        public AddressComponent()
        {
            NeighBorHood = new NeighBorHood();
            Building = new Building();
            StreetNumber = new StreetNumber();
            BusinessAreas = new List<BusinessArea>();
        }
    }

    /// <summary>
    /// 社区信息
    /// </summary>
    public class NeighBorHood
    {
        /// <summary>
        /// 社区名称
        /// 例如：北京大学
        /// </summary>
        public string[] Name { get; set; } = new string[0];

        /// <summary>
        /// POI类型
        /// 例如：科教文化服务;学校;高等院校
        /// </summary>
        public string[] Type { get; set; } = new string[0];
    }
    /// <summary>
    /// 楼信息
    /// </summary>
    public class Building
    {
        /// <summary>
        /// 建筑名称
        /// 例如：万达广场
        /// </summary>
        public string[] Name { get; set; } = new string[0];

        /// <summary>
        /// 类型
        /// 例如：科教文化服务;学校;高等院校
        /// </summary>
        public string[] Type { get; set; } = new string[0];
    }
    /// <summary>
    /// 街道信息
    /// </summary>
    public class StreetNumber
    {
        /// <summary>
        /// 街道名称
        /// 例如：中关村北二条
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 门牌号
        /// 例如：3号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 坐标点
        /// 经纬度坐标点：经度，纬度
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 方向
        /// 坐标点所处街道方位
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// 门牌地址到请求坐标的距离
        /// 单位：米
        /// </summary>
        public string Distance { get; set; }
    }

    /// <summary>
    /// 商圈信息
    /// </summary>
    public class BusinessArea
    {
        /// <summary>
        /// 商圈中心点经纬度
        /// 经纬度坐标点：经度，纬度
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 商圈名称 
        /// 例如：颐和园
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商圈所在区域的adcode  
        /// 例如：朝阳区/海淀区 
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// 道路信息
    /// </summary>
    public class Road
    {
        /// <summary>
        /// 道路Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 道路名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 道路到请求的坐标距离
        /// 单位：米
        /// </summary>
        public string Distance { get; set; }
        /// <summary>
        /// 方位
        /// 输入点和此路的相对方位
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 坐标点
        /// 经纬度坐标点：经度，纬度
        /// </summary>
        public string Location { get; set; }
    }

    /// <summary>
    /// 道路交叉口
    /// </summary>
    public class Roadinter
    {
        /// <summary>
        /// 交叉路口到请求坐标的距离
        /// 单位：米
        /// </summary>
        public string Distance { get; set; }
        /// <summary>
        /// 方位
        /// 输入点相对路口的方位
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 路口经纬度
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 第一条道路id
        /// </summary>
        [JsonProperty("first_id")]
        public string FirstId { get; set; }
        /// <summary>
        /// 第一条道路名称
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        /// <summary>
        /// 第二条道路id
        /// </summary>
        [JsonProperty("second_id")]
        public string SecondId { get; set; }
        /// <summary>
        /// 第二条道路名称
        /// </summary>
        [JsonProperty("second_name")]
        public string SecondName { get; set; }
    }

    /// <summary>
    /// poi信息
    /// </summary>
    public class Poi
    {
        /// <summary>
        /// Poi信息
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// poi点名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// poi类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 该POI的中心点到请求坐标的距离
        /// 单位：米
        /// </summary>
        public string Distance { get; set; }
        /// <summary>
        /// 方向
        /// 为输入点相对建筑物的方位
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// poi地址信息
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 坐标点
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// poi所在商圈名称
        /// </summary>
        public string BusinessArea { get; set; }
    }
    /// <summary>
    /// aoi信息
    /// </summary>
    public class Aoi
    {
        /// <summary>
        /// 所属 aoi的id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 所属 aoi 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所属 aoi 所在区域编码
        /// </summary>
        public string AdCode { get; set; }
        /// <summary>
        /// 所属 aoi 中心点坐标
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 所属aoi点面积
        /// 单位：平方米
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 输入经纬度是否在aoi面之中
        /// 0，代表在aoi内 其余整数代表距离AOI的距离
        /// </summary>
        public string Distance { get; set; }
    }
}
