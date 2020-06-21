namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public static class AliyunBlobProviderConfigurationNames
    {
        /// <summary>
        /// 数据中心
        /// </summary>
        public const string Endpoint = "Aliyun:OSS:Endpoint";
        /// <summary>
        /// 命名空间
        /// </summary>
        public const string BucketName = "Aliyun:OSS:BucketName";
        /// <summary>
        /// 命名空间不存在是否创建
        /// </summary>
        public const string CreateBucketIfNotExists = "Aliyun:OSS:CreateBucketIfNotExists";
        /// <summary>
        /// 创建命名空间时防盗链列表
        /// </summary>
        public const string CreateBucketReferer = "Aliyun:OSS:CreateBucketReferer";
    }
}
