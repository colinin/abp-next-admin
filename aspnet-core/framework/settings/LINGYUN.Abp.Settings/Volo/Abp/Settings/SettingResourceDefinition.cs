using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Localization;

namespace Volo.Abp.Settings;

public class SettingResourceDefinition
{
    public int Order { get; set; }
    public string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? ResourceName { get; set; }
    public string? ResourceType { get; set; }
    public string[]? RequiredFeatures { get; set; }
    public string[]? RequiredPermissions { get; set; }

    public SettingResourceDefinition(
        string name,
        FixedLocalizableString displayName,
        int order = 0,
        string[]? requiredFeatures = null,
        string[]? requiredPermissions = null)
    {
        Name = name;
        Order = order;
        DisplayName = displayName.Value;
        RequiredFeatures = requiredFeatures;
        RequiredPermissions = requiredPermissions;
    }

    public SettingResourceDefinition(
        string name,
        LocalizableString displayName,
        int order = 0,
        string[]? requiredFeatures = null,
        string[]? requiredPermissions = null)
    {
        Name = name;
        Order = order;
        ResourceName = displayName.ResourceName;
        ResourceType = displayName.ResourceType?.FullName;
        DisplayName = displayName.Name;
        RequiredFeatures = requiredFeatures;
        RequiredPermissions = requiredPermissions;
    }

    public LocalizedString Localize(IStringLocalizerFactory stringLocalizerFactory)
    {
        if (!string.IsNullOrWhiteSpace(ResourceName) && 
            !string.IsNullOrWhiteSpace(DisplayName))
        {
            if (!string.IsNullOrWhiteSpace(ResourceType))
            {
                return new LocalizableString(DisplayName!, ResourceType).Localize(stringLocalizerFactory);
            }
            return new LocalizableString(DisplayName!, ResourceName).Localize(stringLocalizerFactory);
        }
        else if (!string.IsNullOrWhiteSpace(DisplayName))
        {
            return new LocalizedString(Name!, DisplayName!);
        }
        return new LocalizedString(Name, Name);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendFormat("N:{0}", Name);
        sb.AppendFormat(",D:{0}", DisplayName ?? string.Empty);
        sb.AppendFormat(",L:{0}", ResourceName ?? string.Empty);
        sb.AppendFormat(",T:{0}", ResourceType ?? string.Empty);
        sb.AppendFormat(",F:{0}", RequiredFeatures?.JoinAsString(";") ?? string.Empty);
        sb.AppendFormat(",P:{0}", RequiredPermissions?.JoinAsString(";") ?? string.Empty);
        sb.AppendFormat(",O:{0}", Order);

        return sb.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj is SettingResourceDefinition other)
        {
            return string.Equals(Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return StringComparer.CurrentCultureIgnoreCase.GetHashCode(Name);
    }

    public static bool operator ==(SettingResourceDefinition? left, SettingResourceDefinition? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(SettingResourceDefinition? left, SettingResourceDefinition? right)
    {
        return !(left == right);
    }
}
