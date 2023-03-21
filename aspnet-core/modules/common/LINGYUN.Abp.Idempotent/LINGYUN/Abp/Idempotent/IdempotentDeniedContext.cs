using System.Collections.Generic;
using System.Reflection;

namespace LINGYUN.Abp.Idempotent;
public class IdempotentDeniedContext
{
    public IdempotentAttribute? Attribute { get; }
    public string IdempotentKey { get; }
    public MethodInfo Method { get; }
    public IReadOnlyDictionary<string, object?> ArgumentsDictionary { get; }
    public int HttpStatusCode { get; set; }
    public IdempotentDeniedContext(
        string idempotentKey,
        IdempotentAttribute? attribute, 
        MethodInfo method, 
        IReadOnlyDictionary<string, object?> argumentsDictionary)
    {
        IdempotentKey = idempotentKey;
        Attribute = attribute;
        Method = method;
        ArgumentsDictionary = argumentsDictionary;
    }
}
