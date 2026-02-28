using System.Collections.Generic;

namespace LINGYUN.Abp.SystemInfo.Models;
/// <summary>
/// 组件
/// </summary>
public class ComponentInfoModel
{
    /// <summary>
    /// 组件名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 组件名称集合
    /// </summary>
    public ComponentKeyModel[] Keys { get; set; }
    /// <summary>
    /// 组件状态集合
    /// </summary>
    public Dictionary<string, object> Details { get; set; }
    public ComponentInfoModel(string name, ComponentKeyModel[] keys, Dictionary<string, object> details)
    {
        Name = name;
        Keys = keys;
        Details = details;
    }
}
