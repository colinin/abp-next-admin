using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.Notifications
{
    public static class NotificationDataMappingDictionaryItemExtensions
    {
        public static NotificationDataMappingDictionaryItem GetOrNullDefault(
            this IEnumerable<NotificationDataMappingDictionaryItem> items,
            string name)
        {
            var item = items.FirstOrDefault(i => i.Name.Equals(name));
            if (item == null)
            {
                return items.FirstOrDefault(i => i.Name.Equals(NotificationDataMappingDictionary.DefaultKey));
            }
            return item;
        }
    }
}
