namespace LINGYUN.Abp.BlobStoring.Qiniu
{
    public static class QiniuBlobProviderConfigurationNames
    {
        /// <summary>
        /// Api授权码
        /// </summary>
        public const string AccessKey = "Qiniu:OSS:AccessKey";
        /// <summary>
        /// Api密钥
        /// </summary>
        public const string SecretKey = "Qiniu:OSS:SecretKey";
        /// <summary>
        /// 默认的Bucket名称
        /// </summary>
        public const string BucketName = "Qiniu:OSS:BucketName";
        /// <summary>
        /// 默认自动删除该文件天数
        /// 默认 0，不删除
        /// </summary>
        public const string DeleteAfterDays = "Qiniu:OSS:DeleteAfterDays";
        /// <summary>
        /// 上传成功后，七牛云向业务服务器发送 POST 请求的 URL。
        /// 必须是公网上可以正常进行 POST 请求并能响应 HTTP/1.1 200 OK 的有效 URL
        /// </summary>
        public const string UploadCallbackUrl = "Qiniu:OSS:UploadCallbackUrl";
        /// <summary>
        /// 上传成功后，七牛云向业务服务器发送回调通知时的 Host 值。
        /// 与 callbackUrl 配合使用，仅当设置了 callbackUrl 时才有效。
        /// </summary>
        public const string UploadCallbackHost = "Qiniu:OSS:UploadCallbackHost";
        /// <summary>
        /// 上传成功后，七牛云向业务服务器发送回调通知 callbackBody 的 Content-Type。
        /// 默认为 application/x-www-form-urlencoded，也可设置为 application/json。
        /// </summary>
        public const string UploadCallbackBodyType = "Qiniu:OSS:UploadCallbackBodyType";
        /// <summary>
        /// 上传成功后，自定义七牛云最终返回給上传端（在指定 returnUrl 时是携带在跳转路径参数中）的数据。
        /// 支持魔法变量和自定义变量。returnBody 要求是合法的 JSON 文本。
        /// 例如 {"key": $(key), "hash": $(etag), "w": $(imageInfo.width), "h": $(imageInfo.height)}。
        /// </summary>
        public const string UploadCallbackBody = "Qiniu:OSS:UploadCallbackBody";
    }
}
