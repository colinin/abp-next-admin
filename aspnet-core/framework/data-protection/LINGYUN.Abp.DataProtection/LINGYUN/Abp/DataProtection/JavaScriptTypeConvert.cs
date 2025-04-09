using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.DataProtection;

public class JavaScriptTypeConvert : IJavaScriptTypeConvert, ISingletonDependency
{
    public JavaScriptTypeConvertResult Convert(Type propertyType)
    {
        var (JavaScriptType, AccessOperates) =  InnerConvert(propertyType);

        return new JavaScriptTypeConvertResult(JavaScriptType, AccessOperates);
    }

    protected virtual (string JavaScriptType, DataAccessFilterOperate[] AccessFilterOperates) InnerConvert(Type propertyType)
    {
        var availableComparator = new List<DataAccessFilterOperate>();
        if (propertyType.IsNullableType())
        {
            propertyType = propertyType.GetGenericArguments().FirstOrDefault();
        }

        if (typeof(Enum).IsAssignableFrom(propertyType))
        {
            // 枚举类型只支持如下操作符
            // 小于、小于等于、大于、大于等于、等于、不等于、空、非空
            availableComparator.AddRange(new[]
            {
                DataAccessFilterOperate.Greater,
                DataAccessFilterOperate.GreaterOrEqual,
                DataAccessFilterOperate.Less,
                DataAccessFilterOperate.LessOrEqual,
                DataAccessFilterOperate.Equal,
                DataAccessFilterOperate.NotEqual,
            });
            return ("number", availableComparator.ToArray());
        }

        var typeFullName = propertyType.FullName;

        switch (typeFullName)
        {
            case "System.Int16":
            case "System.Int32":
            case "System.Int64":
            case "System.UInt16":
            case "System.UInt32":
            case "System.UInt64":
            case "System.Single":
            case "System.Double":
            case "System.Byte":
            case "System.SByte":
            case "System.Decimal":
                // 数值类型只支持如下操作符
                // 小于、小于等于、大于、大于等于、等于、不等于、空、非空
                availableComparator.AddRange(new[]
                {
                    DataAccessFilterOperate.Greater,
                    DataAccessFilterOperate.GreaterOrEqual,
                    DataAccessFilterOperate.Less,
                    DataAccessFilterOperate.LessOrEqual,
                    DataAccessFilterOperate.Equal,
                    DataAccessFilterOperate.NotEqual,
                });
                return ("number", availableComparator.ToArray());
            case "System.Boolean":
                // 布尔类型只支持如下操作符
                // 等于、不等于、空、非空
                availableComparator.AddRange(new[]
                {
                    DataAccessFilterOperate.Equal,
                    DataAccessFilterOperate.NotEqual,
                });
                return ("boolean", availableComparator.ToArray());
            case "System.Guid":
                // Guid类型只支持如下操作符
                // 等于、不等于、空、非空
                availableComparator.AddRange(new[]
                {
                    DataAccessFilterOperate.Equal,
                    DataAccessFilterOperate.NotEqual,
                });
                return ("string", availableComparator.ToArray());
            case "System.Char":
            case "System.String":
                // 字符类型支持所有操作符
                return ("string", availableComparator.ToArray());
            case "System.DateTime":
                // 时间类型只支持如下操作符
                // 小于、小于等于、大于、大于等于、等于、不等于、空、非空
                availableComparator.AddRange(new[]
                {
                    DataAccessFilterOperate.Greater,
                    DataAccessFilterOperate.GreaterOrEqual,
                    DataAccessFilterOperate.Less,
                    DataAccessFilterOperate.LessOrEqual,
                    DataAccessFilterOperate.Equal,
                    DataAccessFilterOperate.NotEqual,
                });
                return ("Date", availableComparator.ToArray());
            default:
            case "System.Object":
            case "System.DBNull":
                if (propertyType.IsArray)
                {
                    // 数组类型只支持如下操作符
                    // 包含、不包含、空、非空
                    availableComparator.AddRange(new[]
                    {
                        DataAccessFilterOperate.Contains,
                        DataAccessFilterOperate.NotContains,
                    });

                    return ("array", availableComparator.ToArray());
                }
                else
                {
                    // 未知对象类型只支持如下操作符
                    // 等于、不等于、空、非空
                    availableComparator.AddRange(new[]
                   {
                        DataAccessFilterOperate.Equal,
                        DataAccessFilterOperate.NotEqual,
                    });
                    return ("object", availableComparator.ToArray());
                }
        }
    }
}
