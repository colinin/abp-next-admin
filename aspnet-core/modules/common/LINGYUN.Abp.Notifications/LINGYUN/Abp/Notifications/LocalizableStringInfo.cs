using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications
{
    public class LocalizableStringInfo
    {
        public string ResourceName { get; }

        public string Name { get; }

        public Dictionary<object, object> Values { get; }

        public LocalizableStringInfo(
            string resourceName, 
            string name,
            Dictionary<object, object> values = null)
        {
            ResourceName = resourceName;
            Name = name;
            Values = values;
        }
    }
}
