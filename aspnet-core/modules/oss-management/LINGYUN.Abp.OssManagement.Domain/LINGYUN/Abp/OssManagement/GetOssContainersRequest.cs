namespace LINGYUN.Abp.OssManagement
{
    public class GetOssContainersRequest
    {
        public string Prefix { get; }
        public string Marker { get; }
        public int Current { get; }
        public int? MaxKeys { get; }
        public GetOssContainersRequest(
            string prefix = null,
            string marker = null,
            int current = 0,
            int? maxKeys = 10)
        {
            Prefix = prefix;
            Marker = marker;
            Current = current;
            MaxKeys = maxKeys;
        }
    }
}
