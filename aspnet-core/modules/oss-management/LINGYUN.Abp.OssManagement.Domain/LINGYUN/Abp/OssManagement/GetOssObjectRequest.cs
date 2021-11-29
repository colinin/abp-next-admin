using JetBrains.Annotations;
using Volo.Abp;

namespace LINGYUN.Abp.OssManagement
{
    public class GetOssObjectRequest
    {
        public string Bucket { get; }
        public string Path { get; }
        public string Object { get; }
        public bool MD5 { get; set; }
        /// <summary>
        /// 需要处理文件的参数
        /// </summary>
        public string Process { get; }

        public bool CreatePathIsNotExists { get; set; } = false;

        public GetOssObjectRequest(
            [NotNull] string bucket,
            [NotNull] string @object,
            [CanBeNull] string path = "",
            [CanBeNull] string process = "")
        {
            Check.NotNullOrWhiteSpace(bucket, nameof(bucket));
            Check.NotNullOrWhiteSpace(@object, nameof(@object));

            Bucket = bucket;
            Object = @object;
            Path = path;
            Process = process;
        }
    }
}
