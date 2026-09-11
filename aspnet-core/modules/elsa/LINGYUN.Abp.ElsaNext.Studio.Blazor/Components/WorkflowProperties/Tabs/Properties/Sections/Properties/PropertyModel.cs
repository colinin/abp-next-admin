namespace LINGYUN.Abp.ElsaNext.Studio.Blazor.Components.WorkflowProperties.Tabs.Properties.Sections.Properties;

public class PropertyModel
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    public PropertyModel()
    {

    }

    public PropertyModel(string key, string value)
    {
        Key = key;
        Value = value;
    }
}
