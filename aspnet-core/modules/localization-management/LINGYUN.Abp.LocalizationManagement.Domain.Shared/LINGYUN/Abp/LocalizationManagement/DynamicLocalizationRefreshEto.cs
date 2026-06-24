namespace LINGYUN.Abp.LocalizationManagement;

public class DynamicLanguageRefreshEventData
{
    public string CultureName { get; set; }
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
    public string ResourceName { get; set; }
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
    public string ResourceName { get; set; }
    public string CultureName { get; set; }
    public DynamicTextRefreshEventData()
    {

    }

    public DynamicTextRefreshEventData(string resourceName, string cultureName)
    {
        ResourceName = resourceName;
        CultureName = cultureName;
    }
}