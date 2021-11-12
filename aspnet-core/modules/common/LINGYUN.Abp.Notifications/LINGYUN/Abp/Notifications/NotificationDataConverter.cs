using LINGYUN.Abp.RealTime.Localization;
using Newtonsoft.Json;

namespace LINGYUN.Abp.Notifications
{
    public class NotificationDataConverter
    {
        public static NotificationData Convert(NotificationData notificationData)
        {
            if (notificationData != null)
            {
                if (notificationData.NeedLocalizer())
                {
                    // 潜在的空对象引用修复
                    if (notificationData.ExtraProperties.TryGetValue("title", out object title) && title != null)
                    {
                        var titleObj = JsonConvert.DeserializeObject<LocalizableStringInfo>(title.ToString());
                        notificationData.TrySetData("title", titleObj);
                    }
                    if (notificationData.ExtraProperties.TryGetValue("message", out object message) && message != null)
                    {
                        var messageObj = JsonConvert.DeserializeObject<LocalizableStringInfo>(message.ToString());
                        notificationData.TrySetData("message", messageObj);
                    }

                    if (notificationData.ExtraProperties.TryGetValue("description", out object description) && description != null)
                    {
                        notificationData.TrySetData("description", JsonConvert.DeserializeObject<LocalizableStringInfo>(description.ToString()));
                    }
                }
            }
            else
            {
                notificationData = new NotificationData();
            }
            return notificationData;
        }
    }
}
