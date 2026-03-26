using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.AI.Tools;
public static class FunctionAIToolDefinitionExtenssions
{
    private const string FunctionType = "Function";
    public static AIToolDefinition WithFunction<TFunc>(this AIToolDefinition definition)
    {
        return definition.WithFunction(typeof(TFunc));
    }

    public static AIToolDefinition WithFunction(this AIToolDefinition definition, Type funcType)
    {
        Check.NotNull(funcType, nameof(funcType));

        definition.WithProperty(FunctionType, funcType);

        return definition;
    }

    public static Type GetFunction(this AIToolDefinition definition)
    {
        var funcTypeSet = definition.Properties.GetOrDefault(FunctionType);
        Check.NotNull(funcTypeSet, nameof(FunctionType));

        var funcType = Type.GetType(funcTypeSet.ToString()!);

        Check.NotNull(funcType, nameof(funcType));

        return funcType;
    }
}
