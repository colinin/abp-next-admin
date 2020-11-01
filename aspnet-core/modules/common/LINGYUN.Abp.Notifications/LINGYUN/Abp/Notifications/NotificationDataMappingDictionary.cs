using System;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDataMappingDictionary : Dictionary<string, List<NotificationDataMappingDictionaryItem>>
    {
        public void Mapping(string name, string provider, Func<NotificationData, NotificationData> func)
        {
            if (ContainsKey(name))
            {
                this[name] = new List<NotificationDataMappingDictionaryItem>();
            }
            this[name].Add(new NotificationDataMappingDictionaryItem(provider, func));
        }

        public void MappingAll(string provider, Func<NotificationData, NotificationData> func)
        {
            foreach(var mapping in this)
            {
                Mapping(mapping.Key, provider, func);
            }
        }

        public NotificationDataMappingDictionaryItem GetMapItemOrNull(string name, string provider)
        {
            if (ContainsKey(name))
            {
                return this[name].FirstOrDefault(map => map.Provider.Equals(provider));
            }
            return null;
        }
    }
}
