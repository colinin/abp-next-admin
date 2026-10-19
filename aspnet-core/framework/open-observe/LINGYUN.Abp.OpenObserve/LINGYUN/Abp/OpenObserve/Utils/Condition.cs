namespace LINGYUN.Abp.OpenObserve.Utils;

/// <summary>
/// 查询条件
/// </summary>
/// <param name="Field">字段名(表达式成员名)</param>
/// <param name="Op">比较操作符</param>
/// <param name="Value">条件值</param>
public record Condition(string Field, ComparisonOp Op, object? Value);
