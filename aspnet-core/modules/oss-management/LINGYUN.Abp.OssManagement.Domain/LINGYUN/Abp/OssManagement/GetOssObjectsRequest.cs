using JetBrains.Annotations;
using Volo.Abp;

namespace LINGYUN.Abp.OssManagement
{
    public class GetOssObjectsRequest
    {
        public string BucketName { get; }
        public string Prefix { get; }
        public string Delimiter { get; }
        public string Marker { get; }
        public string EncodingType { get; }
        public int Current { get; }
        public int? MaxKeys { get; }
        public bool MD5 { get; set; }
        public bool CreatePathIsNotExists { get; set; } = false;
        public GetOssObjectsRequest(
            [NotNull] string bucketName,
            string prefix = null,
            string marker = null,
            string delimiter = null,
            string encodingType = null,
            int current = 0,
            int maxKeys = 10)
        {
            Check.NotNullOrWhiteSpace(bucketName, nameof(bucketName));

            BucketName = bucketName;
            Prefix = prefix;
            Marker = marker;
            Delimiter = delimiter;
            EncodingType = encodingType;
            Current = current;
            MaxKeys = maxKeys;
        }
    }
}
