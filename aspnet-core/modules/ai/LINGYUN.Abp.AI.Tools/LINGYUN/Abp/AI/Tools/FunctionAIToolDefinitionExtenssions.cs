using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.AI.Tools;
public static class FunctionAIToolDefinitionExtenssions
{
    public const string FunctionType = "FunctionType";
    public const string FunctionName = "FunctionName";
    public static AIToolDefinition WithFunction<TFunc>(this AIToolDefinition definition, string? funcName = null)
    {
        return definition.WithFunction(typeof(TFunc), funcName);
    }

    public static AIToolDefinition WithFunction(this AIToolDefinition definition, Type funcType, string? funcName = null)
    {
        Check.NotNull(funcType, nameof(funcType));

        definition.WithProperty(FunctionType, funcType.AssemblyQualifiedName);
        if (!funcName.IsNullOrWhiteSpace())
        {
            definition.WithProperty(FunctionName, funcName);
        }

        return definition;
    }

    public static Type GetFunctionType(this AIToolDefinition definition)
    {
        var funcTypeSet = definition.Properties.GetOrDefault(FunctionType);
        Check.NotNull(funcTypeSet, nameof(FunctionType));

        var funcType = Type.GetType(funcTypeSet.ToString()!);

        Check.NotNull(funcType, nameof(funcType));

        return funcType;
    }

    public static string? GetFunctionNameOrNull(this AIToolDefinition definition)
    {
        var funcNameSet = definition.Properties.GetOrDefault(FunctionName);

        return funcNameSet?.ToString();
    }
}
