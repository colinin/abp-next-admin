using Volo.Abp;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Qiniu
{
    public class QiniuBlobProviderConfiguration
    {
        /// <summary>
        /// Api授权码
        /// </remarks>
        public string AccessKey
        {
            get => _containerConfiguration.GetConfiguration<string>(QiniuBlobProviderConfigurationNames.AccessKey);
            set => _containerConfiguration.SetConfiguration(QiniuBlobProviderConfigurationNames.AccessKey, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }
        /// <summary>
        /// Api密钥
        /// </summary>
        public string SecretKey
        {
            get => _containerConfiguration.GetConfiguration<string>(QiniuBlobProviderConfigurationNames.SecretKey);
            set => _containerConfiguration.SetConfiguration(QiniuBlobProviderConfigurationNames.SecretKey, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }
        /// <summary>
        /// 默认的Bucket名称
        /// </summary>
        public string BucketName
        {
            get => _containerConfiguration.GetConfigurationOrDefault(QiniuBlobProviderConfigurationNames.BucketName, "");
            set => _containerConfiguration.SetConfiguration(QiniuBlobProviderConfigurationNames.BucketName, value ?? "");
        }
        /// <summary>
        /// 默认自动删除该文件天数
        /// 默认 0，不删除
        /// </summary>
        public int DeleteAfterDays
        {
            get => _containerConfiguration.GetConfigurationOrDefault(QiniuBlobProviderConfigurationNames.DeleteAfterDays, 0);
            set => _containerConfiguration.SetConfiguration(QiniuBlobProviderConfigurationNames.DeleteAfterDays, value);
        }
        /// <summary>
        /// 上传成功后，七牛云向业务服务器发送 POST 请求的 URL。
        /// 必须是公网上可以正常进行 POST 请求并能响应 HTTP/1.1 200 OK 的有效 URL
        /// </summary>
        public string UploadCallbackUrl
        {
            get => _containerConfiguration.GetConfigurationOrDefault(QiniuBlobProviderConfigurationNames.UploadCallbackUrl, "");
            set => _containerConfiguration.SetConfiguration(QiniuBlobProviderConfigurationNames.UploadCallbackUrl, value ?? "");
        }
        /// <summary>
        /// 上传成功后，七牛云向业务服务器发送回调通知时的 Host 值。
        /// 与 callbackUrl 配合使用，仅当设置了 callbackUrl 时才有效。
        /// </summary>
        public string UploadCallbackHost
        {
            get => _containerConfiguration.GetConfigurationOrDefault(QiniuBlobProviderConfigurationNames.UploadCallbackHost, "");
            set => _containerConfiguration.SetConfiguration(QiniuBlobProviderConfigurationNames.UploadCallbackHost, value ?? "");
        }
        /// <summary>
        /// 上传成功后，七牛云向业务服务器发送回调通知 callbackBody 的 Content-Type。
        /// 默认为 application/x-www-form-urlencoded，也可设置为 application/json。
        /// </summary>
        public string UploadCallbackBodyType
        {
            get => _containerConfiguration.GetConfigurationOrDefault(QiniuBlobProviderConfigurationNames.UploadCallbackBodyType, "");
            set => _containerConfiguration.SetConfiguration(QiniuBlobProviderConfigurationNames.UploadCallbackBodyType, value ?? "");
        }
        /// <summary>
        /// 上传成功后，自定义七牛云最终返回給上传端（在指定 returnUrl 时是携带在跳转路径参数中）的数据。
        /// 支持魔法变量和自定义变量。returnBody 要求是合法的 JSON 文本。
        /// 例如 {"key": $(key), "hash": $(etag), "w": $(imageInfo.width), "h": $(imageInfo.height)}。
        /// </summary>
        public string UploadCallbackBody
        {
            get => _containerConfiguration.GetConfigurationOrDefault(QiniuBlobProviderConfigurationNames.UploadCallbackBody, "");
            set => _containerConfiguration.SetConfiguration(QiniuBlobProviderConfigurationNames.UploadCallbackBody, value ?? "");
        }
        private readonly BlobContainerConfiguration _containerConfiguration;

        public QiniuBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
