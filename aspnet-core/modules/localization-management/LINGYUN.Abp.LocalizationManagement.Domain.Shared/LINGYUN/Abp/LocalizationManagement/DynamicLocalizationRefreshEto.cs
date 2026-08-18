namespace LINGYUN.Abp.LocalizationManagement;

public class DynamicLanguageRefreshEventData
{
    public string CultureName { get; set; } = default!;
    public DynamicLanguageRefreshEventData()
    {

    }

    public DynamicLanguageRefreshEventData(string cultureName)
    {
        CultureName = cultureName;
    }
}

public class DynamicResourceRefreshEventData
{
    public string ResourceName { get; set; } = default!;
    public DynamicResourceRefreshEventData()
    {

    }

    public DynamicResourceRefreshEventData(string resourceName)
    {
        ResourceName = resourceName;
    }
}

public class DynamicTextRefreshEventData
{
    public string ResourceName { get; set; } = default!;
    public string CultureName { get; set; } = default!;
    public DynamicTextRefreshEventData()
    {

    }

    public DynamicTextRefreshEventData(string resourceName, string cultureName)
    {
        ResourceName = resourceName;
        CultureName = cultureName;
    }
}