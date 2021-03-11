namespace LINGYUN.Abp.OssManagement
{
    public class GetOssContainersRequest
    {
        public string Prefix { get; }
        public string Marker { get; }
        public int? MaxKeys { get; }
        public GetOssContainersRequest(
            string prefix = null,
            string marker = null,
            int? maxKeys = 10)
        {
            Prefix = prefix;
            Marker = marker;
            MaxKeys = maxKeys;
        }
    }
}
