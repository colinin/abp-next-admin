namespace LINGYUN.Abp.BlobStoring.Tencent
{
    public static class TencentBlobProviderConfigurationNames
    {
        /// <summary>
        /// AppId
        /// </summary>
        public const string AppId = "Tencent:OSS:AppId";
        /// <summary>
        /// 区域
        /// </summary>
        public const string Region = "Tencent:OSS:Region";
        /// <summary>
        /// 命名空间
        /// </summary>
        public const string BucketName = "Tencent:OSS:BucketName";
        /// <summary>
        /// 命名空间不存在是否创建
        /// </summary>
        public const string CreateBucketIfNotExists = "Tencent:OSS:CreateBucketIfNotExists";
        /// <summary>
        /// 创建命名空间时防盗链列表
        /// </summary>
        public const string CreateBucketReferer = "Tencent:OSS:CreateBucketReferer";
    }
}
