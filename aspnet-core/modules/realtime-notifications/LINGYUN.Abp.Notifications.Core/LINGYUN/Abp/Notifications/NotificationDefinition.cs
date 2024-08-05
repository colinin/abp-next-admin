using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

/*
 * 2020-10-29 重构通知
 * INotificationSender指定接收者,未指定才会查询所有订阅用户,已指定发布者,直接发布(检验是否订阅)
 */

namespace LINGYUN.Abp.Notifications;

public class NotificationDefinition
{
    /// <summary>
    /// 通知名称
    /// </summary>
    [NotNull]
    public string Name { get; set; }
    /// <summary>
    /// 通知显示名称
    /// </summary>
    [NotNull]
    public ILocalizableString DisplayName
    {
        get => _displayName;
        set => _displayName = Check.NotNull(value, nameof(value));
    }
    private ILocalizableString _displayName;
    /// <summary>
    /// 通知说明
    /// </summary>
    [CanBeNull]
    public ILocalizableString Description { get; set; }
    /// <summary>
    /// 允许客户端显示订阅
    /// </summary>
    public bool AllowSubscriptionToClients { get; set; }
    /// <summary>
    /// 存活类型
    /// </summary>
    public NotificationLifetime NotificationLifetime { get; set; }
    /// <summary>
    /// 通知类型
    /// </summary>
    public NotificationType NotificationType { get; set; }
    /// <summary>
    /// 通知内容类型
    /// </summary>
    public NotificationContentType ContentType { get; set; }
    /// <summary>
    /// 通知提供者
    /// </summary>
    public List<string> Providers { get; }
    /// <summary>
    /// 通知模板
    /// </summary>
    public TemplateDefinition Template { get; private set; }
    /// <summary>
    /// 额外属性
    /// </summary>
    [NotNull]
    public Dictionary<string, object> Properties { get; }

    public object this[string name] {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    public NotificationDefinition(
       string name,
       ILocalizableString displayName = null,
       ILocalizableString description = null,
       NotificationType notificationType = NotificationType.Application,
       NotificationLifetime lifetime = NotificationLifetime.Persistent,
       NotificationContentType contentType = NotificationContentType.Text,
       bool allowSubscriptionToClients = false)
    {
        Name = name;
        DisplayName = displayName ?? new FixedLocalizableString(name);
        Description = description;
        NotificationLifetime = lifetime;
        NotificationType = notificationType;
        ContentType = contentType;
        AllowSubscriptionToClients = allowSubscriptionToClients;

        Providers = new List<string>();
        Properties = new Dictionary<string, object>();
    }

    public virtual NotificationDefinition WithProviders(params string[] providers)
    {
        if (!providers.IsNullOrEmpty())
        {
            Providers.AddIfNotContains(providers);
        }

        return this;
    }

    public virtual NotificationDefinition WithTemplate(
        Type localizationResource = null, 
        bool isLayout = false, 
        string layout = null, 
        string defaultCultureName = null)
    {
        Template = new TemplateDefinition(
            Name,
            localizationResource,
            DisplayName,
            isLayout,
            layout,
            defaultCultureName);

        return this;
    }

    public virtual NotificationDefinition WithTemplate(Action<TemplateDefinition> setup)
    {
        if (Template != null)
        {
            setup(Template);
        }
        else
        {
            var template = new TemplateDefinition(Name);
            setup(template);

            Template = template;
        }

        return this;
    }

    public virtual NotificationDefinition WithTemplate(TemplateDefinition template)
    {
        Template = template;

        return this;
    }

    public virtual NotificationDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    public override string ToString()
    {
        return $"[{nameof(NotificationDefinition)} {Name}]";
    }
}
