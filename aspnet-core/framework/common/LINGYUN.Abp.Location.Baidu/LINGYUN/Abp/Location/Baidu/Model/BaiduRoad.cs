namespace LINGYUN.Abp.Location.Baidu.Model
{
    /// <summary>
    /// 道路
    /// </summary>
    public class BaiduRoad 
    {
        public string Name { get; set; }
        /// <summary>
        /// 传入的坐标点距离道路的大概距离
        /// </summary>
        public string Distance { get; set; }
    }
}
