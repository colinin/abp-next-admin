using System;
using System.Collections.Generic;
using System.Reflection;

namespace LINGYUN.Abp.Idempotent;

public class IdempotentKeyNormalizerContext
{
    public Type Target { get; }
    public MethodInfo Method { get; }
    public IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }

    public IdempotentKeyNormalizerContext(
        Type target,
        MethodInfo method,
        IReadOnlyDictionary<string, object>? argumentsDictionary)
    {
        Target = target;
        Method = method;
        ArgumentsDictionary = argumentsDictionary ?? new Dictionary<string, object>();
    }
}
