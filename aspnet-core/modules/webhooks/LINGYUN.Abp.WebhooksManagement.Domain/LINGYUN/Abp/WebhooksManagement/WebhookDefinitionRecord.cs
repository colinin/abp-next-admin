using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public override Guid Id { get; protected set; }

    public string GroupName { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public bool IsEnabled { get; set; }

    public string RequiredFeatures { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public WebhookDefinitionRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public WebhookDefinitionRecord(
        Guid id,
        string groupName,
        string name,
        string displayName,
        string description = null,
        bool isEnabled = true,
        string requiredFeatures = null)
        : base(id)
    {
        GroupName = Check.NotNullOrWhiteSpace(groupName, nameof(groupName), WebhookGroupDefinitionRecordConsts.MaxNameLength);
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), WebhookDefinitionRecordConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), WebhookDefinitionRecordConsts.MaxDisplayNameLength);
        Description = Check.Length(description, nameof(description), WebhookDefinitionRecordConsts.MaxDescriptionLength);
        RequiredFeatures = Check.Length(requiredFeatures, nameof(requiredFeatures), WebhookDefinitionRecordConsts.MaxRequiredFeaturesLength);

        IsEnabled = isEnabled;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public bool HasSameData(WebhookDefinitionRecord otherRecord)
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

        if (Description != otherRecord.Description)
        {
            return false;
        }

        if (IsEnabled != otherRecord.IsEnabled)
        {
            return false;
        }

        if (RequiredFeatures != otherRecord.RequiredFeatures)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    public void Patch(WebhookDefinitionRecord otherRecord)
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

        if (Description != otherRecord.Description)
        {
            Description = otherRecord.Description;
        }

        if (IsEnabled != otherRecord.IsEnabled)
        {
            IsEnabled = otherRecord.IsEnabled;
        }

        if (RequiredFeatures != otherRecord.RequiredFeatures)
        {
            RequiredFeatures = otherRecord.RequiredFeatures;
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
