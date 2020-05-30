using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationData
    {
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
    }
}
