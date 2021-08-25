using Newtonsoft.Json;
using System.Collections.Generic;

namespace LINGYUN.Abp.WeChat.MiniProgram.Messages
{
    public class SubscribeMessage
    {
        /// <summary>
        /// 接收者（用户）的 openid
        /// </summary>
        [JsonProperty("touser")]
        public string ToUser { get; set; }
        /// <summary>
        /// 所需下发的订阅模板id
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }
        /// <summary>
        /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。
        /// 支持带参数,（示例index?foo=bar）。
        /// 该字段不填则模板无跳转
        /// </summary>
        [JsonProperty("page")]
        public string Page { get; set; }
        /// <summary>
        /// 跳转小程序类型：
        /// developer为开发版；trial为体验版；formal为正式版；
        /// 默认为正式版
        /// </summary>
        [JsonProperty("miniprogram_state")]
        public string MiniProgramState { get; set; }
        /// <summary>
        /// 进入小程序查看”的语言类型，
        /// 支持zh_CN(简体中文)、en_US(英文)、zh_HK(繁体中文)、zh_TW(繁体中文)，
        /// 默认为zh_CN
        /// </summary>
        [JsonProperty("lang")]
        public string Lang { get; set; } = "zh_CN";
        /// <summary>
        /// 模板内容，
        /// 格式形如 { "key1": { "value": any }, "key2": { "value": any } }
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string, MessageData> Data { get; set; } = new Dictionary<string, MessageData>();

        public SubscribeMessage() { }
        public SubscribeMessage(
            string openId,
            string templateId,
            string redirectPage = "",
            string state = "formal",
            string miniLang = "zh_CN")
        {
            ToUser = openId;
            TemplateId = templateId;
            Page = redirectPage;
            MiniProgramState = state;
            Lang = miniLang;
        }

        public SubscribeMessage WriteData(string prefix, string key, object value)
        {
            // 只截取符合标记的数据
            if (key.StartsWith(prefix))
            {
                key = key.Replace(prefix, "");
                if (!Data.ContainsKey(key))
                {
                    Data.Add(key, new MessageData(value));
                }
            }
            return this;
        }

        public SubscribeMessage WriteData(string prefix, IDictionary<string, object> setData)
        {
            foreach (var kv in setData)
            {
                WriteData(prefix, kv.Key, kv.Value);
            }
            return this;
        }

        public SubscribeMessage WriteData(IDictionary<string, object> setData)
        {
            foreach (var kv in setData)
            {
                if (!Data.ContainsKey(kv.Key))
                {
                    Data.Add(kv.Key, new MessageData(kv.Value));
                }
            }
            return this;
        }
    }

    public class MessageData
    {
        public object Value { get; }

        public MessageData(object value)
        {
            Value = value;
        }
    }
}
