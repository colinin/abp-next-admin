using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Linq.Expressions;

namespace LINGYUN.Abp.Elasticsearch;

public partial class ExpressionQueryTranslator
{
    /// <summary>
    /// 翻译 Enum.HasFlag
    /// </summary>
    private Query TranslateEnumHasFlag(MethodCallExpression node, string? prefix, IndexMappingInfo? mappingInfo)
    {
        var field = ResolveField(node.Object!, prefix, mappingInfo);
        var flag = Evaluate(node.Arguments[0]);

        if (flag == null)
        {
            throw new NotSupportedException("Cannot use null flag in Enum.HasFlag");
        }

        var flagValue = Convert.ToInt64(flag);
        return new TermQuery { Field = field.Path, Value = flagValue };
    }
}
