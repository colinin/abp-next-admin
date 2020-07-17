namespace LINGYUN.Abp.Location.Tencent
{
    public class TencentLocationOptions
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string GetPoi { get; set; } = "1";
        public string Output { get; set; } = "JSON";
        public string Callback { get; set; }
        public bool VisableErrorToClient { get; set; } = false;
    }
}
