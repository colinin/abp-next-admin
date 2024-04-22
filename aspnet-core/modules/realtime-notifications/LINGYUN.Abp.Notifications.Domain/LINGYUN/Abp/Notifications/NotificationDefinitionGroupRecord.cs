using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.Notifications;

public class NotificationDefinitionGroupRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    /// <summary>
    /// 分组名称
    /// </summary>
    public virtual string Name { get; set; }
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
    /// 允许客户端订阅
    /// </summary>
    public virtual bool AllowSubscriptionToClients { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public NotificationDefinitionGroupRecord() 
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public NotificationDefinitionGroupRecord(
        Guid id,
        string name,
        string displayName = null,
        string description = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), NotificationDefinitionGroupRecordConsts.MaxNameLength);
        DisplayName = Check.Length(displayName, nameof(displayName), NotificationDefinitionGroupRecordConsts.MaxDisplayNameLength);
        Description = Check.Length(description, nameof(description), NotificationDefinitionGroupRecordConsts.MaxDescriptionLength);

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();

        AllowSubscriptionToClients = true;
    }

    public bool HasSameData(NotificationDefinitionGroupRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            return false;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            return false;
        }

        if (Description != otherRecord.Description)
        {
            return false;
        }

        if (AllowSubscriptionToClients != otherRecord.AllowSubscriptionToClients)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    public void Patch(NotificationDefinitionGroupRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            Name = otherRecord.Name;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
        }

        if (Description != otherRecord.Description)
        {
            Description = otherRecord.Description;
        }

        if (AllowSubscriptionToClients != otherRecord.AllowSubscriptionToClients)
        {
            AllowSubscriptionToClients = otherRecord.AllowSubscriptionToClients;
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
