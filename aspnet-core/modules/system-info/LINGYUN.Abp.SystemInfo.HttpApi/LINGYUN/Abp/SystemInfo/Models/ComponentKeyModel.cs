namespace LINGYUN.Abp.SystemInfo.Models;
/// <summary>
/// 组件名称
/// </summary>
public class ComponentKeyModel
{
    /// <summary>
    /// 组件名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 组件显示名称
    /// </summary>
    public string DisplayName { get; set; }
    public ComponentKeyModel(string name, string displayName)
    {
        Name = name;
        DisplayName = displayName;
    }
}
