namespace LINGYUN.Abp.Localization.Persistence;
public class LocalizableStringText
{
    public LocalizableStringText(
        string resourceName, 
        string cultureName, 
        string name, 
        string value)
    {
        ResourceName = resourceName;
        CultureName = cultureName;
        Name = name;
        Value = value;
    }

    public string ResourceName { get; set; }
    public string CultureName { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public override string ToString()
    {
        return $"[R]:{ResourceName},[C]:{CultureName ?? ""},[N]:{Name},[V]:{Value ?? ""}";
    }
}
