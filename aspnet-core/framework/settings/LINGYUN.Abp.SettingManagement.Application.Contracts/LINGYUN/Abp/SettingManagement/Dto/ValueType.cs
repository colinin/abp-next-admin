namespace LINGYUN.Abp.SettingManagement;

public enum ValueType
{
    /// <summary>
    /// 不加入设置管理(配合前端请勿传递设置值)
    /// </summary>
    NoSet = -1,
    /// <summary>
    /// 字符
    /// </summary>
    String = 0,
    /// <summary>
    /// 数字
    /// </summary>
    Number = 1,
    /// <summary>
    /// 布尔
    /// </summary>
    Boolean = 2,
    /// <summary>
    /// 日期
    /// </summary>
    Date = 3,
    /// <summary>
    /// 数组
    /// </summary>
    Array = 4,
    /// <summary>
    /// 选项
    /// </summary>
    Option = 5,
    /// <summary>
    /// 对象
    /// </summary>
    Object = 10
}
