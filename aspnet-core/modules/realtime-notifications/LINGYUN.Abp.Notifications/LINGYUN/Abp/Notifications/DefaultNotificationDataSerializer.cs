﻿using LINGYUN.Abp.RealTime.Localization;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

[Dependency(TryRegister = true)]
public class DefaultNotificationDataSerializer : INotificationDataSerializer, ISingletonDependency
{
    public NotificationData Serialize(NotificationData source)
    {
        if (source != null)
        {
            if (source.NeedLocalizer())
            {
                // 潜在的空对象引用修复
                if (source.ExtraProperties.TryGetValue("title", out var title) && title != null)
                {
                    var titleObj = JsonConvert.DeserializeObject<LocalizableStringInfo>(title.ToString());
                    source.TrySetData("title", titleObj);
                }
                if (source.ExtraProperties.TryGetValue("message", out var message) && message != null)
                {
                    var messageObj = JsonConvert.DeserializeObject<LocalizableStringInfo>(message.ToString());
                    source.TrySetData("message", messageObj);
                }

                if (source.ExtraProperties.TryGetValue("description", out var description) && description != null)
                {
                    var descriptionObj = JsonConvert.DeserializeObject<LocalizableStringInfo>(description.ToString());
                    source.TrySetData("description", descriptionObj);
                }
            }
        }
        else
        {
            source = new NotificationData();
        }
        return source;
    }
}
