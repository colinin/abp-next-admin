using LINGYUN.Abp.RealTime.Localization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications;

[Dependency(TryRegister = true)]
public class DefaultNotificationDataSerializer : INotificationDataSerializer, ISingletonDependency
{
    private readonly IStringLocalizerFactory _localizerFactory;
    public DefaultNotificationDataSerializer(IStringLocalizerFactory localizerFactory)
    {
        _localizerFactory = localizerFactory;
    }

    public virtual NotificationData Serialize(NotificationData source)
    {
        if (source != null)
        {
            if (source.NeedLocalizer())
            {
                // 潜在的空对象引用修复
                if (source.ExtraProperties.TryGetValue("title", out var title) && 
                    title != null &&
                    title is not LocalizableStringInfo)
                {
                    var titleObj = JsonConvert.DeserializeObject<LocalizableStringInfo>(title.ToString());
                    source.TrySetData("title", titleObj);
                }
                if (source.ExtraProperties.TryGetValue("message", out var message) && 
                    message != null &&
                    message is not LocalizableStringInfo)
                {
                    var messageObj = JsonConvert.DeserializeObject<LocalizableStringInfo>(message.ToString());
                    source.TrySetData("message", messageObj);
                }

                if (source.ExtraProperties.TryGetValue("description", out var description) && 
                    description != null &&
                    description is not LocalizableStringInfo)
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

    public async virtual Task<NotificationStandardData> ToStandard(NotificationData source)
    {
        var title = "";
        var message = "";
        var description = "";

        if (!source.NeedLocalizer())
        {
            title = source.TryGetData("title").ToString();
            message = source.TryGetData("message").ToString();
            description = source.TryGetData("description")?.ToString() ?? "";
        }
        else
        {
            var titleInfo = source.TryGetData("title").As<LocalizableStringInfo>();
            var titleLocalizer = await _localizerFactory.CreateByResourceNameAsync(titleInfo.ResourceName);
            title = titleLocalizer[titleInfo.Name].Value;
            if (titleInfo.Values != null)
            {
                foreach (var formatValue in titleInfo.Values)
                {
                    if (formatValue.Key != null && formatValue.Value != null)
                    {
                        title = title.Replace($"{{{formatValue.Key}}}", formatValue.Value.ToString());
                    }
                }
            }
            var messageInfo = source.TryGetData("message").As<LocalizableStringInfo>();
            var messageLocalizer = await _localizerFactory.CreateByResourceNameAsync(messageInfo.ResourceName); 
            message = messageLocalizer[messageInfo.Name].Value;
            if (messageInfo.Values != null)
            {
                foreach (var formatValue in messageInfo.Values)
                {
                    if (formatValue.Key != null && formatValue.Value != null)
                    {
                        message = message.Replace($"{{{formatValue.Key}}}", formatValue.Value.ToString());
                    }
                }
            }
            var descriptionInfo = source.TryGetData("description")?.As<LocalizableStringInfo>();
            if (descriptionInfo != null)
            {
                var descriptionLocalizer = await _localizerFactory.CreateByResourceNameAsync(descriptionInfo.ResourceName);
                description = descriptionLocalizer[descriptionInfo.Name].Value;
                if (descriptionInfo.Values != null)
                {
                    foreach (var formatValue in descriptionInfo.Values)
                    {
                        if (formatValue.Key != null && formatValue.Value != null)
                        {
                            description = description.Replace($"{{{formatValue.Key}}}", formatValue.Value.ToString());
                        }
                    }
                }
            }
        }

        return new NotificationStandardData(title, message, description);
    }
}
