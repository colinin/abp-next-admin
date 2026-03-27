using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.AI.Tools;

public enum PropertyValueType
{
    String = 0,
    Number = 1,
    Boolean = 2,
    Dictionary = 3,
    Select = 4,
}

public class AIToolPropertyDescriptor
{
    public string Name { get; }
    public bool Required { get; }
    public PropertyValueType ValueType { get; }
    public List<NameValue<object>> Options { get; }
    public ILocalizableString DisplayName { get; }
    public ILocalizableString? Description { get; }
    private AIToolPropertyDescriptor(
        string name,
        PropertyValueType valueType, 
        ILocalizableString displayName, 
        ILocalizableString? description = null,
        bool required = false)
    {
        Name = name;
        ValueType = valueType;
        DisplayName = displayName;
        Description = description;
        Required = required;

        Options = new List<NameValue<object>>();
    }

    public static AIToolPropertyDescriptor CreateStringProperty(
        string name,
        ILocalizableString displayName,
        ILocalizableString? description = null,
        bool required = false)
    {
        return new AIToolPropertyDescriptor(
            name,
            PropertyValueType.String,
            displayName,
            description,
            required);
    }

    public static AIToolPropertyDescriptor CreateNumberProperty(
        string name,
        ILocalizableString displayName,
        ILocalizableString? description = null,
        bool required = false)
    {
        return new AIToolPropertyDescriptor(
            name,
            PropertyValueType.Number,
            displayName,
            description,
            required);
    }

    public static AIToolPropertyDescriptor CreateBoolProperty(
        string name,
        ILocalizableString displayName,
        ILocalizableString? description = null,
        bool required = false)
    {
        return new AIToolPropertyDescriptor(
            name,
            PropertyValueType.Boolean,
            displayName,
            description,
            required);
    }

    public static AIToolPropertyDescriptor CreateDictionaryProperty(
        string name,
        ILocalizableString displayName,
        ILocalizableString? description = null)
    {
        return new AIToolPropertyDescriptor(
            name,
            PropertyValueType.Dictionary,
            displayName,
            description);
    }

    public static AIToolPropertyDescriptor CreateSelectProperty(
        string name,
        ILocalizableString displayName,
        List<NameValue<object>> options,
        ILocalizableString? description = null,
        bool required = false)
    {
        var propertyDescriptor = new AIToolPropertyDescriptor(
            name,
            PropertyValueType.Select,
            displayName,
            description,
            required);

        foreach (var option in options)
        {
            propertyDescriptor.WithOption(option.Name, option.Value);
        }

        return propertyDescriptor;
    }

    public void WithOption(string name, object value)
    {
        Options.Add(new NameValue<object>(name, value));
    }
}
