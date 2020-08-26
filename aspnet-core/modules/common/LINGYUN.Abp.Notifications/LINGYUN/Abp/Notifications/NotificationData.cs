using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationData
    {
        public const string NotificationKey = "N:G";
        public const string UserIdNotificationKey = "N:UI";
        public const string UserNameNotificationKey = "N:UN";
        public const string TenantNotificationKey = "N:T";
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
        }

        public static NotificationData CreateNotificationData()
        {
            var data = new NotificationData();
            data.TrySetData(NotificationKey, "AbpNotification");
            return data;
        }

        public static NotificationData CreateUserNotificationData(Guid userId, string userName)
        {
            var data = new NotificationData();
            data.TrySetData(UserIdNotificationKey, userId);
            data.TrySetData(UserNameNotificationKey, userName);
            return data;
        }

        public static NotificationData CreateTenantNotificationData(Guid tenantId)
        {
            var data = new NotificationData();
            data.TrySetData(TenantNotificationKey, tenantId);
            return data;
        }

        /// <summary>
        /// 写入标准数据
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        /// <param name="createTime">创建时间</param>
        /// <param name="formUser">来源用户</param>
        /// <returns></returns>
        public NotificationData WriteStandardData(string title, string message, DateTime createTime, string formUser)
        {
            TrySetData("title", title);
            TrySetData("message", message);
            TrySetData("formUser", formUser);
            TrySetData("createTime", createTime);
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
            data.TrySetData("formUser", sourceData.TryGetData("formUser"));
            data.TrySetData("createTime", sourceData.TryGetData("createTime"));
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
                Properties[key] = value;
            }
        }

        public bool HasUserNotification(out Guid userId, out string userName)
        {
            if (Properties.TryGetValue(UserIdNotificationKey, out object userKey))
            {
                userId = (Guid)userKey;
                var name = TryGetData(UserNameNotificationKey);
                userName = name != null ? name.ToString() : "";
                return true;
            }
            userName = "";
            return false;
        }

        public bool HasTenantNotification(out Guid tenantId)
        {
            if (Properties.TryGetValue(TenantNotificationKey, out object tenantKey))
            {
                tenantId = (Guid)tenantKey;
                return true;
            }
            return false;
        }
    }
}
