using Newtonsoft.Json;
using Volo.Abp;

namespace LINGYUN.Abp.WeChat.MiniProgram.Messages
{
    public class SubscribeMessageResponse
    {
        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }

        [JsonProperty("errmsg")]
        public string ErrorMessage { get; set; }

        public bool IsSuccessed => ErrorCode == 0;

        public void ThrowIfNotSuccess()
        {
            if (ErrorCode != 0)
            {
                throw new AbpException($"Send wechat weapp notification error:{ErrorMessage}");
            }
        }
    }
}
