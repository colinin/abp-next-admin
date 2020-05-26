namespace LINGYUN.Abp.Location.Baidu.Http
{
    public abstract class BaiduLocationResponse
    {
        public int Status { get; set; }

        public Location Location { get; set; }

        public bool IsSuccess()
        {
            return Status == 0;
        }
    }
}
