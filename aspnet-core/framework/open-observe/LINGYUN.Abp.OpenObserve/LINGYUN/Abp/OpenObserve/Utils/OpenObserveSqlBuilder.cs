using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LINGYUN.Abp.OpenObserve.Utils;

/// <summary>
/// OpenObserve SQL 条件构建器
/// </summary>
/// <remarks>
/// 将 <see cref="Condition"/> 列表转换为 SQL WHERE 条件, 字段名可通过
/// <see cref="MapField"/> 映射(表达式成员名 -> 数据流列名)
/// </remarks>
public class OpenObserveSqlBuilder
{
    protected Dictionary<string, string> FieldMappings { get; }

    public OpenObserveSqlBuilder()
    {
        FieldMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 添加字段映射
    /// </summary>
    public virtual OpenObserveSqlBuilder MapField(string field, string column)
    {
        FieldMappings[field] = column;

        return this;
    }

    /// <summary>
    /// 获取映射后的列名
    /// </summary>
    public virtual string GetMappedField(string field)
    {
        return FieldMappings.TryGetValue(field, out var column) ? column : field;
    }

    /// <summary>
    /// 构建 SQL WHERE 条件
    /// </summary>
    /// <param name="conditions">查询条件</param>
    /// <param name="skipFields">需要忽略的字段(通常由查询的时间范围处理)</param>
    public virtual string Build(IEnumerable<Condition> conditions, params string[] skipFields)
    {
        var parts = new List<string>();

        foreach (var condition in conditions)
        {
            if (IsSkip(condition.Field, skipFields))
            {
                continue;
            }

            parts.Add(BuildCondition(condition));
        }

        return parts.Count == 0 ? "1 = 1" : string.Join(" AND ", parts);
    }

    /// <summary>
    /// 构建单个条件
    /// </summary>
    protected virtual string BuildCondition(Condition condition)
    {
        var field = GetMappedField(condition.Field);
        var value = FormatValue(condition.Field, condition.Value);

        return condition.Op switch
        {
            ComparisonOp.Equal => $"{field} = {value}",
            ComparisonOp.NotEqual => $"{field} != {value}",
            ComparisonOp.GreaterThan => $"{field} > {value}",
            ComparisonOp.GreaterThanOrEqual => $"{field} >= {value}",
            ComparisonOp.LessThan => $"{field} < {value}",
            ComparisonOp.LessThanOrEqual => $"{field} <= {value}",
            ComparisonOp.Contains => $"{field} LIKE '%{Escape(condition.Value?.ToString() ?? string.Empty)}%'",
            ComparisonOp.StartsWith => $"{field} LIKE '{Escape(condition.Value?.ToString() ?? string.Empty)}%'",
            ComparisonOp.EndsWith => $"{field} LIKE '%{Escape(condition.Value?.ToString() ?? string.Empty)}'",
            ComparisonOp.IsNull => $"{field} IS NULL",
            ComparisonOp.IsNotNull => $"{field} IS NOT NULL",
            _ => throw new NotSupportedException($"Unsupported comparison operator: {condition.Op}")
        };
    }

    /// <summary>
    /// 格式化条件值
    /// </summary>
    protected virtual string FormatValue(string field, object? value)
    {
        return value switch
        {
            null => "NULL",
            string s => $"'{Escape(s)}'",
            bool b => b ? "true" : "false",
            DateTime d => FormatDateTime(d),
            DateTimeOffset d => $"'{d:yyyy-MM-ddTHH:mm:ss.fffffffzzz}'",
            Guid g => $"'{g}'",
            Enum e => $"'{Escape(e.ToString())}'",
            _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? "NULL"
        };
    }

    /// <summary>
    /// 格式化时间值
    /// </summary>
    /// <remarks>
    /// OpenObserve 将时间序列化为 ISO 8601(带时区偏移), 按相同格式比较才能命中
    /// </remarks>
    public virtual string FormatDateTime(DateTime value)
    {
        return $"'{value:yyyy-MM-ddTHH:mm:ss.fffffffzzz}'";
    }

    /// <summary>
    /// 转义字符串值
    /// </summary>
    public virtual string Escape(string input)
    {
        return input.Replace("'", "''");
    }

    protected virtual bool IsSkip(string field, string[] skipFields)
    {
        foreach (var skip in skipFields)
        {
            if (string.Equals(field, skip, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 追加条件到 SQL
    /// </summary>
    public virtual void AppendCondition(StringBuilder sqlBuilder, string column, string sqlValue, bool isLike = false)
    {
        if (isLike)
        {
            sqlBuilder.AppendFormat(" and {0} like '%{1}%'", column, Escape(sqlValue));
        }
        else
        {
            sqlBuilder.AppendFormat(" and {0} = '{1}'", column, Escape(sqlValue));
        }
    }
}
