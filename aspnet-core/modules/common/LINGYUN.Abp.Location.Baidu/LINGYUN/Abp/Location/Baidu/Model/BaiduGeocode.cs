namespace LINGYUN.Abp.Location.Baidu.Model
{
    public class BaiduGeocode
    {
        public BaiduLocation Location { get; set; } = new BaiduLocation();
        public int Precise { get; set; }
        public int Confidence { get; set; }
        public int Comprehension { get; set; }
        public string Level { get; set; }
    }
}
