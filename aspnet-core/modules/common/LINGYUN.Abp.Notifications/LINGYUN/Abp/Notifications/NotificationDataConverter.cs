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
                    var title = JsonConvert.DeserializeObject<LocalizableStringInfo>(notificationData.TryGetData("title").ToString());
                    var message = JsonConvert.DeserializeObject<LocalizableStringInfo>(notificationData.TryGetData("message").ToString());
                    notificationData.TrySetData("title", title);
                    notificationData.TrySetData("message", message);

                    if (notificationData.Properties.TryGetValue("description", out object description) && description != null)
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
