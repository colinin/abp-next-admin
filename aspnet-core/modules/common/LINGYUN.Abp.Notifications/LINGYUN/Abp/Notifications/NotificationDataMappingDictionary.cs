using System;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDataMappingDictionary : Dictionary<string, List<NotificationDataMappingDictionaryItem>>
    {
        public void Mapping(string cateGory, string provider, Func<NotificationData, NotificationData> func)
        {
            if (ContainsKey(cateGory))
            {
                this[cateGory] = new List<NotificationDataMappingDictionaryItem>();
            }
            this[cateGory].Add(new NotificationDataMappingDictionaryItem(provider, func));
        }

        public void MappingAll(string provider, Func<NotificationData, NotificationData> func)
        {
            foreach(var mapping in this)
            {
                Mapping(mapping.Key, provider, func);
            }
        }

        public NotificationDataMappingDictionaryItem GetMapItemOrNull(string cateGory, string provider)
        {
            if (ContainsKey(cateGory))
            {
                return this[cateGory].FirstOrDefault(map => map.Provider.Equals(provider));
            }
            return null;
        }
    }
}
