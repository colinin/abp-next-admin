using LINGYUN.Abp.RealTime.Localization;
using System;
using Volo.Abp.Data;
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
    public class NotificationData : IHasExtraProperties
    {
        /// <summary>
        /// 用来标识是否需要本地化的信息
        /// </summary>
        public const string LocalizerKey = "L";

        public virtual string Type => GetType().FullName;

        public object this[string key]
        {
            get
            {
                return this.GetProperty(key);
            }
            set
            {
                this.SetProperty(key, value);
            }
        }

        public ExtraPropertyDictionary ExtraProperties { get; set; }

        public NotificationData()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();

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

            foreach (var property in sourceData.ExtraProperties)
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
            return this.GetProperty(key);
        }
        public void TrySetData(string key, object value)
        {
            this.SetProperty(key, value);
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
