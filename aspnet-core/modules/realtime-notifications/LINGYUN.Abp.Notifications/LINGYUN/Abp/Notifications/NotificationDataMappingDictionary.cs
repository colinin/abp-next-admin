using System;
using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDataMappingDictionary : Dictionary<string, List<NotificationDataMappingDictionaryItem>>
    {
        public static string DefaultKey { get; set; } = "Default";
        /// <summary>
        /// 处理某个通知的数据
        /// 特定于一个提供程序
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="name"></param>
        /// <param name="func"></param>
        public void Mapping(string provider, string name, Func<NotificationData, NotificationData> func)
        {
            if (!ContainsKey(provider))
            {
                this[provider] = new List<NotificationDataMappingDictionaryItem>();
            }

            var mapItem = this[provider].FirstOrDefault(item => item.Name.Equals(name));

            if (mapItem == null)
            {
                this[provider].Add(new NotificationDataMappingDictionaryItem(name, func));
            }
            else
            {
                mapItem.Replace(func);
            }
        }
        /// <summary>
        /// 处理所有通知的数据
        /// 特定于一个提供程序
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="func"></param>
        public void MappingDefault(string provider, Func<NotificationData, NotificationData> func)
        {
            Mapping(provider, DefaultKey, func);
        }
        /// <summary>
        /// 获取需要处理数据的方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public NotificationDataMappingDictionaryItem GetMapItemOrDefault(string provider, string name)
        {
            if (ContainsKey(provider))
            {
                return this[provider].GetOrNullDefault(name);
            }
            return null;
        }
    }
}
