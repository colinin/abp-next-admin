using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Localization;

namespace Volo.Abp.Settings;

public static class SettingDefinitionExtensions
{
    private const string GroupKey = "Group";
    private const string ParentKey = "Parent";
    private const string OrderKey = "Order";
    private const string SlotKey = "Slot";
    private const string ValueTypeKey = "ValueType";
    private const string OptionNameKey = "OptionNames";
    private const string OptionValueKey = "OptionValues";
    private const string RequiredFeaturesKey = "RequiredFeatures";
    private const string RequiredPermissionsKey = "RequiredPermissions";

    public static SettingDefinition RequiredFeatures(
        this SettingDefinition definition,
        IEnumerable<string> requiredFeatures)
    {
        var existsRequiredFeatures = definition.GetRequiredFeatures();
        return definition
            .WithProperty(RequiredFeaturesKey, existsRequiredFeatures.Union(requiredFeatures).JoinAsString(","));
    }

    public static SettingDefinition RequiredPermissions(
        this SettingDefinition definition,
        IEnumerable<string> requiredPermissions)
    {
        var existsRequiredPermissions = definition.GetRequiredPermissions();
        return definition
            .WithProperty(RequiredPermissionsKey, existsRequiredPermissions.Union(requiredPermissions).JoinAsString(","));
    }

    public static SettingDefinition WithSlot(
        this SettingDefinition definition,
        string slotName)
    {
        return definition
            .WithProperty(SlotKey, slotName);
    }

    public static SettingDefinition WithValueType(
        this SettingDefinition definition,
        ValueType valueType)
    {
        return definition
            .WithProperty(ValueTypeKey, valueType.ToString());
    }

    public static SettingDefinition WithOrder(
        this SettingDefinition definition,
        int order)
    {
        return definition
            .WithProperty(OrderKey, order.ToString());
    }

    public static SettingDefinition WithGroup(
        this SettingDefinition definition, 
        string name,
        string displayName,
        int order = 0,
        string[]? requiredFeatures = null,
        string[]? requiredPermissions = null)
    {
        var groupDefinition = new SettingResourceDefinition(
            name,
            new FixedLocalizableString(displayName),
            order,
            requiredFeatures,
            requiredPermissions);

        return definition
            .WithProperty(GroupKey, groupDefinition.ToString());
    }

    public static SettingDefinition WithGroup(
        this SettingDefinition definition,
        string name,
        LocalizableString displayName,
        int order = 0,
        string[]? requiredFeatures = null,
        string[]? requiredPermissions = null)
    {
        var groupDefinition = new SettingResourceDefinition(
            name,
            displayName,
            order,
            requiredFeatures,
            requiredPermissions);

        return definition
            .WithProperty(GroupKey, groupDefinition.ToString());
    }

    public static SettingDefinition RequiredGroupFeatures(
        this SettingDefinition definition,
        string[] requiredFeatures)
    {
        var groupResource = definition.GetGroupOrNull();
        if (groupResource != null)
        {
            groupResource.RequiredFeatures = requiredFeatures
                .Union(groupResource.RequiredFeatures ?? [])
                .ToArray();

            return definition
                .WithProperty(GroupKey, groupResource.ToString());
        }

        return definition;
    }

    public static SettingDefinition RequiredGroupPermissions(
        this SettingDefinition definition,
        string[] requiredPermissions)
    {
        var groupResource = definition.GetGroupOrNull();
        if (groupResource != null)
        {
            groupResource.RequiredPermissions = requiredPermissions
                .Union(groupResource.RequiredPermissions ?? [])
                .ToArray();

            return definition
                .WithProperty(GroupKey, groupResource.ToString());
        }

        return definition;
    }

    public static SettingDefinition WithParent(
        this SettingDefinition definition,
        string name,
        string displayName,
        int order = 0,
        string[]? requiredFeatures = null,
        string[]? requiredPermissions = null)
    {
        var parentDefinition = new SettingResourceDefinition(
            name,
            new FixedLocalizableString(displayName),
            order,
            requiredFeatures,
            requiredPermissions);

        return definition
            .WithProperty(ParentKey, parentDefinition.ToString());
    }

    public static SettingDefinition WithParent(
        this SettingDefinition definition,
        string name,
        LocalizableString displayName,
        int order = 0,
        string[]? requiredFeatures = null,
        string[]? requiredPermissions = null)
    {
        var parentDefinition = new SettingResourceDefinition(
            name,
            displayName,
            order,
            requiredFeatures,
            requiredPermissions);

        return definition
            .WithProperty(ParentKey, parentDefinition.ToString());
    }

    public static SettingDefinition WithOptions(
        this SettingDefinition definition,
        IEnumerable<NameValue<string>> options)
    {
        var optionNames = options.Select(x => x.Name).JoinAsString(",");
        var optionValues = options.Select(x => x.Value).JoinAsString(",");

        return definition
            .WithValueType(ValueType.Option)
            .WithProperty(OptionNameKey, optionNames)
            .WithProperty(OptionValueKey, optionValues);
    }

    public static SettingDefinition ReplaceProviders(
        this SettingDefinition definition, 
        params string[] providers)
    {
        definition.Providers.Clear();

        return definition.WithProviders(providers);
    }

    public static string? GetSlotOrNull(this SettingDefinition definition)
    {
        if (definition.Properties.TryGetValue(SlotKey, out var slot) && slot != null)
        {
            return slot.ToString();
        }

        return null;
    }

    public static ValueType GetValueTypeOrDefault(this SettingDefinition definition, ValueType defaultValueType = ValueType.String)
    {
        if (definition.Properties.TryGetValue(ValueTypeKey, out var valueTypeStr) && valueTypeStr != null &&
            Enum.TryParse<ValueType>(valueTypeStr.ToString(), out var valueType))
        {
            return valueType;
        }

        return defaultValueType;
    }

    public static SettingResourceDefinition? GetGroupOrNull(this SettingDefinition definition)
    {
        return GetResourceOrNull(definition, GroupKey);
    }

    public static SettingResourceDefinition? GetParentOrNull(this SettingDefinition definition)
    {
        return GetResourceOrNull(definition, ParentKey);
    }

    public static IEnumerable<NameValue<string>> GetOptions(this SettingDefinition definition)
    {
        if (definition.TryGetArrayProperties(OptionNameKey, out var optionNames) &&
            definition.TryGetArrayProperties(OptionValueKey, out var optionValues) &&
            optionNames.Length == optionValues.Length)
        {
            return optionNames.Select((name, index) =>
            {
                return new NameValue<string>(name, optionValues[index]);
            });
        }

        return [];
    }

    public static IEnumerable<string> GetRequiredFeatures(this SettingDefinition definition)
    {
        definition.TryGetArrayProperties(RequiredFeaturesKey, out var requiredFeatures);
        return requiredFeatures;
    }

    public static IEnumerable<string> GetRequiredPermissions(this SettingDefinition definition)
    {
        definition.TryGetArrayProperties(RequiredPermissionsKey, out var requiredPermissions);
        return requiredPermissions;
    }

    private static bool TryGetArrayProperties(this SettingDefinition definition, string propertyKey, out string[] enumerableProps)
    {
        if (definition.Properties.TryGetValue(propertyKey, out var propertyKeyValues) && propertyKeyValues != null)
        {
            enumerableProps = propertyKeyValues.ToString()!.Split(',');
            return true;
        }
        enumerableProps = [];
        return false;
    }

    private static SettingResourceDefinition? GetResourceOrNull(SettingDefinition definition, string resourceKey)
    {
        if (definition.Properties.TryGetValue(resourceKey, out var resource) && resource != null)
        {
            var resourceDefineStr = resource.ToString();
            if (string.IsNullOrWhiteSpace(resourceDefineStr))
            {
                return null;
            }
            var resourceDefineKeys = resourceDefineStr.Split(',');
            if (resourceDefineKeys.Length < 6)
            {
                return null;
            }
            var name = resourceDefineKeys[0].Substring(2);
            var displayName = resourceDefineKeys[1].Substring(2);
            var resourceName = resourceDefineKeys[2];
            SettingResourceDefinition settingResource;
            if (resourceName.Length > 2)
            {
                settingResource = new SettingResourceDefinition(
                    name,
                    new LocalizableString(
                        displayName,
                        resourceName.Substring(2)));
            }
            else
            {
                settingResource = new SettingResourceDefinition(
                    name,
                    new FixedLocalizableString(displayName));
            }
            if (resourceDefineKeys[4].Length > 2)
            {
                settingResource.RequiredFeatures = resourceDefineKeys[4].Substring(2).Split(';');
            }
            if (resourceDefineKeys[5].Length > 2)
            {
                settingResource.RequiredPermissions = resourceDefineKeys[5].Substring(2).Split(';');
            }
            if (resourceDefineKeys[6].Length > 2 && int.TryParse(resourceDefineKeys[6].Substring(2), out var order))
            {
                settingResource.Order = order;
            }

            return settingResource;
        }

        return null;
    }
}
