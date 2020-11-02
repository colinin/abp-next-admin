#pragma warning disable IDE1006 // 禁止编译器提示
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications.WeChat.WeApp
{
    public class WeChatWeAppSendNotificationData
    {
        /// <summary>
        /// 接收者（用户）的 openid
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 所需下发的订阅模板id
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。
        /// 支持带参数,（示例index?foo=bar）。
        /// 该字段不填则模板无跳转
        /// </summary>
        public string page { get; set; }
        /// <summary>
        /// 跳转小程序类型：
        /// developer为开发版；trial为体验版；formal为正式版；
        /// 默认为正式版
        /// </summary>
        public string miniprogram_state { get; set; }
        /// <summary>
        /// 进入小程序查看”的语言类型，
        /// 支持zh_CN(简体中文)、en_US(英文)、zh_HK(繁体中文)、zh_TW(繁体中文)，
        /// 默认为zh_CN
        /// </summary>
        public string lang { get; set; }
        /// <summary>
        /// 模板内容，
        /// 格式形如 { "key1": { "value": any }, "key2": { "value": any } }
        /// </summary>
        public Dictionary<string, WeChatNotificationData> data { get; set; }

        public WeChatWeAppSendNotificationData() { }
        public WeChatWeAppSendNotificationData(string openId, string templateId, string redirectPage = "", 
            string state = "formal", string miniLang = "zh_CN")
        {
            touser = openId;
            template_id = templateId;
            page = redirectPage;
            miniprogram_state = state;
            lang = miniLang;

            data = new Dictionary<string, WeChatNotificationData>();
        }
        /// <summary>
        /// 写入标准数据
        /// </summary>
        /// <param name="writeData"></param>
        /// <returns></returns>
        public WeChatWeAppSendNotificationData WriteStandardData(NotificationData writeData)
        {
            foreach (var kv in writeData.Properties)
            {
                if (!data.ContainsKey(kv.Key))
                {
                    data.Add(kv.Key, new WeChatNotificationData(kv.Value));
                }
            }
            return this;
        }

        public WeChatWeAppSendNotificationData WriteData(string prefix, string key, object value)
        {
            // 只截取符合标记的数据
            if (key.StartsWith(prefix))
            {
                key = key.Replace(prefix, "");
                if (!data.ContainsKey(key))
                {
                    data.Add(key, new WeChatNotificationData(value));
                }
            }
            return this;
        }

        public WeChatWeAppSendNotificationData WriteData(string prefix, IDictionary<string, object> setData)
        {
            foreach(var kv in setData)
            {
                WriteData(prefix, kv.Key, kv.Value);
            }
            return this;
        }
    }

    public class WeChatNotificationData
    {
        public object Value { get; }

        public WeChatNotificationData(object value)
        {
            Value = value;
        }
    }
}
#pragma warning restore IDE1006
