namespace LINGYUN.Abp.Location.Baidu.Http
{
    public class BaiduPositiveLocationResponse : BaiduLocationResponse
    {
        public int Precise { get; set; }
        public int Confidence { get; set; }
        public int Comprehension { get; set; }
        public string Level { get; set; }
    }
}
