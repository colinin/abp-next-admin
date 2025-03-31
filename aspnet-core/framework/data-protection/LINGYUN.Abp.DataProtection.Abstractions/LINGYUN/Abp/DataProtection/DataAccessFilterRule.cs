using System;

namespace LINGYUN.Abp.DataProtection;

[Serializable]
public class DataAccessFilterRule
{
    /// <summary>
    /// 字段名称
    /// </summary>
    public string Field { get; set; }
    /// <summary>
    /// 字段值
    /// </summary>
    public object Value { get; set; }
    /// <summary>
    /// 类型全名
    /// </summary>
    public string TypeFullName { get; set; }
    /// <summary>
    /// Js类型
    /// </summary>
    public string JavaScriptType { get; set; }
    /// <summary>
    /// 操作类型
    /// </summary>
    public DataAccessFilterOperate Operate { get; set; }
    /// <summary>
    /// 左侧条件
    /// </summary>
    public bool IsLeft { get; set; }

    public DataAccessFilterRule()
    {

    }

    public DataAccessFilterRule(
        string field,
        object value,
        string typeFullName,
        string javaScriptType,
        DataAccessFilterOperate operate = DataAccessFilterOperate.Equal, 
        bool isLeft = false)
    {
        Field = field;
        Value = value;
        TypeFullName = typeFullName;
        JavaScriptType = javaScriptType;
        Operate = operate;
        IsLeft = isLeft;
    }
}