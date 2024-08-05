using System.Collections.Generic;

namespace LINGYUN.Abp.RealTime.Localization;

/// <summary>
/// The notification that needs to be localized
/// </summary>
public class LocalizableStringInfo
{
    /// <summary>
    /// Resource name
    /// </summary>
    public string ResourceName { get; set; }
    /// <summary>
    /// Properties
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Formatted data
    /// </summary>
    public Dictionary<object, object> Values { get; set; }
    /// <summary>
    /// Instantiate <see cref="LocalizableStringInfo"/>
    /// </summary>
    public LocalizableStringInfo()
    {
    }
    /// <summary>
    /// Instantiate <see cref="LocalizableStringInfo"/>
    /// </summary>
    /// <param name="resourceName">Resource name</param>
    /// <param name="name">Properties</param>
    /// <param name="values">Formatted data</param>
    public LocalizableStringInfo(
        string resourceName, 
        string name,
        Dictionary<object, object> values = null)
    {
        ResourceName = resourceName;
        Name = name;
        Values = values;
    }
}
