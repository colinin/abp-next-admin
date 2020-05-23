namespace LINYUN.Abp.Sms.Aliyun
{
    public class AliyunSmsOptions
    {
        /// <summary>
        /// 区域Id
        /// </summary>
        public string RegionId { get; set; } = "default";
        /// <summary>
        /// 阿里云sms服务域名
        /// </summary>
        public string Domain { get; set; } = "dysmsapi.aliyuncs.com";
        /// <summary>
        /// 调用方法名称
        /// </summary>
        public string ActionName { get; set; } = "SendSms";
        /// <summary>
        /// ApiKey
        /// </summary>
        public string AccessKeyId { get; set; }
        /// <summary>
        /// Api密钥
        /// </summary>
        public string AccessKeySecret { get; set; }
        /// <summary>
        /// 默认版本号
        /// </summary>
        public string DefaultVersion { get; set; } = "2017-05-25";
        /// <summary>
        /// 默认签名
        /// </summary>
        public string DefaultSignName { get; set; }
        /// <summary>
        /// 默认短信模板号
        /// </summary>
        public string DefaultTemplateCode { get; set; }
        /// <summary>
        /// 开发人员号码,当应用处于开发模式时,默认所有信息都会发送到此号码
        /// </summary>
        public string DeveloperPhoneNumber { get; set; } = "13800138000";
        /// <summary>
        /// 展示错误给客户端
        /// </summary>
        public bool VisableErrorToClient { get; set; } = false;
    }
}
