using System;
using System.Collections.Generic;
using System.Reflection;

namespace LINGYUN.Abp.Idempotent;
public class IdempotentCheckContext
{
    public Type Target { get; }
    public MethodInfo Method { get; }
    public string IdempotentKey { get; }
    public IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }
    public IdempotentCheckContext(
        Type target,
        MethodInfo method,
        string idempotentKey,
        IReadOnlyDictionary<string, object>? argumentsDictionary)
    {
        Target = target;
        Method = method;
        IdempotentKey = idempotentKey;
        ArgumentsDictionary = argumentsDictionary ?? new Dictionary<string, object>();
    }
}
