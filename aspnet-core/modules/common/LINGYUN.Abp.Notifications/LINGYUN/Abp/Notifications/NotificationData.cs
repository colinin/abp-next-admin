using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.Notifications
{
    /// <summary>
    /// 通知数据
    /// </summary>
    /// <remarks>
    /// TODO: 2020-10-29 针对不同语言的用户,如果在发布时期就本地化语言是错误的设计
    /// 把通知的标题和内容设计为 <see cref="LocalizableStringInfo"/> 让客户端自行本地化
    /// </remarks>
    [Serializable]
    [EventName("notifications")]
    public class NotificationData
    {
        /// <summary>
        /// 用来标识是否需要本地化的信息
        /// </summary>
        public const string LocalizerKey = "localizer";

        public virtual string Type => GetType().FullName;

        public object this[string key]
        {
            get
            {
                if(Properties.TryGetValue(key, out object @obj))
                {
                    return @obj;
                }
                return null;
            }
            set { Properties[key] = value; }
        }

        public Dictionary<string, object> Properties
        {
            get { return _properties; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                foreach (var keyValue in value)
                {
                    if (!_properties.ContainsKey(keyValue.Key))
                    {
                        _properties[keyValue.Key] = keyValue.Value;
                    }
                }
            }
        }
        private readonly Dictionary<string, object> _properties;

        public NotificationData()
        {
            _properties = new Dictionary<string, object>();
            TrySetData(LocalizerKey, false);
        }
        /// <summary>
        /// 写入本地化的消息数据
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="createTime"></param>
        /// <param name="formUser"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public NotificationData WriteLocalizedData(
            LocalizableStringInfo title,
            LocalizableStringInfo message,
            DateTime createTime, 
            string formUser,
            LocalizableStringInfo description = null)
        {
            TrySetData("title", title);
            TrySetData("message", message);
            TrySetData("formUser", formUser);
            TrySetData("createTime", createTime);
            TrySetData(LocalizerKey, true);
            if (description != null)
            {
                TrySetData("description", description);
            }
            return this;
        }
        /// <summary>
        /// 写入标准数据
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="formUser">来源用户</param>
        /// <param name="description">附加说明</param>
        /// <returns></returns>
        public NotificationData WriteStandardData(string title, string message, DateTime createTime, string formUser, string description = "")
        {
            TrySetData("title", title);
            TrySetData("message", message);
            TrySetData("description", description);
            TrySetData("formUser", formUser);
            TrySetData("createTime", createTime);
            TrySetData(LocalizerKey, false);
            return this;
        }
        /// <summary>
        /// 写入标准数据
        /// </summary>
        /// <param name="prefix">数据前缀</param>
        /// <param name="key">标识</param>
        /// <param name="value">数据内容</param>
        /// <returns></returns>
        public NotificationData WriteStandardData(string prefix, string key, object value)
        {
            TrySetData(string.Concat(prefix, key), value);
            TrySetData(LocalizerKey, false);
            return this;
        }
        /// <summary>
        /// 转换为标准数据
        /// </summary>
        /// <param name="sourceData">原始数据</param>
        /// <returns></returns>
        public static NotificationData ToStandardData(NotificationData sourceData)
        {
            var data = new NotificationData();
            data.TrySetData("title", sourceData.TryGetData("title"));
            data.TrySetData("message", sourceData.TryGetData("message"));
            data.TrySetData("description", sourceData.TryGetData("description"));
            data.TrySetData("formUser", sourceData.TryGetData("formUser"));
            data.TrySetData("createTime", sourceData.TryGetData("createTime"));
            data.TrySetData(LocalizerKey, sourceData.TryGetData(LocalizerKey));
            return data;
        }
        /// <summary>
        /// 转换为标准数据
        /// </summary>
        /// <param name="prefix">数据前缀</param>
        /// <param name="sourceData">原始数据</param>
        /// <returns></returns>
        public static NotificationData ToStandardData(string prefix, NotificationData sourceData)
        {
            var data = ToStandardData(sourceData);

            foreach(var property in sourceData.Properties)
            {
                if (property.Key.StartsWith(prefix))
                {
                    var key = property.Key.Replace(prefix, "");
                    data.TrySetData(key, property.Value);
                }
            }
            return data;
        }

        public object TryGetData(string key)
        {
            if (Properties.TryGetValue(key, out object value))
            {
                return value;
            }
            return null;
        }
        public void TrySetData(string key, object value)
        {
            if (value != null && !Properties.ContainsKey(key))
            {
                Properties.Add(key, value);
            }
            Properties[key] = value;
        }
        /// <summary>
        /// 需要本地化
        /// </summary>
        /// <returns></returns>
        public bool NeedLocalizer()
        {
            var localizer = TryGetData(LocalizerKey);
            if (localizer != null && localizer is bool needLocalizer)
            {
                return needLocalizer;
            }
            return false;
        }
    }
}
