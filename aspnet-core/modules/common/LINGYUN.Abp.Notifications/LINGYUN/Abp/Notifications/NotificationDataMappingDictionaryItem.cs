using System;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDataMappingDictionaryItem
    {
        public string Provider { get; }

        public Func<NotificationData, NotificationData> MappingFunc { get; }
        public NotificationDataMappingDictionaryItem(string prodiver, Func<NotificationData, NotificationData> func)
        {
            Provider = prodiver;
            MappingFunc = func;
        }
    }
}
