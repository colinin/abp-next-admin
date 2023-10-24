using LINGYUN.Abp.Location.Tencent.Localization;
using Newtonsoft.Json;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Location.Tencent.Response
{
    public abstract class TencentLocationResponse
    {
        /// <summary>
        /// 状态码，0为正常,
        /// 310请求参数信息有误，
        /// 311Key格式错误,
        /// 306请求有护持信息请检查字符串,
        /// 110请求来源未被授权
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }
        /// <summary>
        /// 状态说明
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// 本次请求的唯一标识
        /// </summary>
        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        /// <summary>
        /// 是否请求成功
        /// </summary>
        public bool IsSuccessed => Status.Equals(0);

        public ILocalizableString GetErrorMessage(bool throwToClient = false)
        {
            switch (Status)
            {
                case 0:
                    return LocalizableString.Create<TencentLocationResource>("Message:RETURN_0");
                case 110:
                    return LocalizableString.Create<TencentLocationResource>("Message:RETURN_110");
                case 306:
                    return LocalizableString.Create<TencentLocationResource>("Message:RETURN_306");
                case 310:
                    return LocalizableString.Create<TencentLocationResource>("Message:RETURN_310");
                case 311:
                    return LocalizableString.Create<TencentLocationResource>("Message:RETURN_311");
                default:
                    if (throwToClient)
                    {
                        throw new LocationResolveException(Message);
                    }
                    throw new AbpException(Message);
            }
        }
    }
}
