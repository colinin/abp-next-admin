﻿using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.Notifications;

public class NotificationDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; set; }
    /// <summary>
    /// 分组名称
    /// </summary>
    public virtual string GroupName { get; set; }
    /// <summary>
    /// 显示名称
    /// </summary>
    /// <remarks>
    /// 如果为空,回退到Name
    /// </remarks>
    public virtual string DisplayName { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    /// <remarks>
    /// 如果为空,回退到Name
    /// </remarks>
    public virtual string Description { get; set; }
    /// <summary>
    /// 资源名称
    /// </summary>
    /// <remarks>
    /// 如果不为空,作为参与本地化资源的名称
    /// DisplayName = L["DisplayName:Localization"]
    /// Description = L["Description:Localization"]
    /// </remarks>
    public virtual string ResourceName { get; protected set; }
    /// <summary>
    /// 本地化键值名称
    /// </summary>
    /// <remarks>
    /// 如果不为空,作为参与本地化键值的名称
    /// DisplayName = L["DisplayName:Localization"]
    /// Description = L["Description:Localization"]
    /// </remarks>
    public virtual string Localization { get; protected set; }
    /// <summary>
    /// 存活类型
    /// </summary>
    public virtual NotificationLifetime NotificationLifetime { get; set; }
    /// <summary>
    /// 通知类型
    /// </summary>
    public virtual NotificationType NotificationType { get; set; }
    /// <summary>
    /// 通知内容类型
    /// </summary>
    public virtual NotificationContentType ContentType { get; set; }
    /// <summary>
    /// 通知提供者
    /// </summary>
    /// <remarks>
    /// 多个之间用;分隔
    /// </remarks>
    public virtual string Providers { get; protected set; }
    /// <summary>
    /// 允许客户端订阅
    /// </summary>
    public virtual bool AllowSubscriptionToClients { get; set; }
    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public NotificationDefinitionRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public NotificationDefinitionRecord(
        Guid id,
        string name,
        string groupName,
        string displayName = null,
        string description = null,
        string resourceName = null,
        string localization = null,
        NotificationLifetime lifetime = NotificationLifetime.Persistent,
        NotificationType notificationType = NotificationType.Application,
        NotificationContentType contentType = NotificationContentType.Text)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), NotificationDefinitionRecordConsts.MaxNameLength);
        GroupName = Check.NotNullOrWhiteSpace(groupName, nameof(groupName), NotificationDefinitionGroupRecordConsts.MaxNameLength);
        DisplayName = Check.Length(displayName, nameof(displayName), NotificationDefinitionRecordConsts.MaxDisplayNameLength);
        Description = Check.Length(description, nameof(description), NotificationDefinitionRecordConsts.MaxDescriptionLength);
        NotificationLifetime = lifetime;
        NotificationType = notificationType;
        ContentType = contentType;

        SetLocalization(resourceName, localization);

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();

        AllowSubscriptionToClients = true;
    }
    /// <summary>
    /// 设置本地化资源
    /// </summary>
    /// <param name="resourceName"></param>
    /// <param name="localization"></param>
    public void SetLocalization(string resourceName, string localization)
    {
        ResourceName = Check.Length(resourceName, nameof(resourceName), NotificationDefinitionGroupRecordConsts.MaxResourceNameLength);
        Localization = Check.Length(localization, nameof(localization), NotificationDefinitionGroupRecordConsts.MaxLocalizationLength);
    }

    public void UseProviders(params string[] providers)
    {
        var currentProviders = Providers.IsNullOrWhiteSpace()
            ? new List<string>()
            : Providers.Split(';').ToList();

        if (!providers.IsNullOrEmpty())
        {
            currentProviders.AddIfNotContains(providers);
        }

        if (currentProviders.Any())
        {
            Providers = currentProviders.JoinAsString(";");
        }
    }

    public bool HasSameData(NotificationDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            return false;
        }

        if (GroupName != otherRecord.GroupName)
        {
            return false;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            return false;
        }

        if (Providers != otherRecord.Providers)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    public void Patch(NotificationDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            Name = otherRecord.Name;
        }

        if (GroupName != otherRecord.GroupName)
        {
            GroupName = otherRecord.GroupName;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
        }

        if (Providers != otherRecord.Providers)
        {
            Providers = otherRecord.Providers;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            ExtraProperties.Clear();

            foreach (var property in otherRecord.ExtraProperties)
            {
                ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}