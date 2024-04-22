using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Idempotent;
public class IdempotentKeyNormalizer : IIdempotentKeyNormalizer, ITransientDependency
{
    private const string KeyFormat = "t:{0};m:{1};k:{2}";

    private readonly IJsonSerializer _jsonSerializer;

    public IdempotentKeyNormalizer(
        IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    [IgnoreIdempotent]
    public virtual string NormalizeKey(IdempotentKeyNormalizerContext context)
    {
        var methodIdBuilder = new StringBuilder();

        if (context.Method.IsDefined(typeof(IdempotentAttribute)))
        {
            var attr = context.Method.GetCustomAttribute<IdempotentAttribute>();
            if (!attr.IdempotentKey.IsNullOrWhiteSpace())
            {
                return attr.IdempotentKey!;
            }
            if (attr.KeyMap != null)
            {
                var index = 0;
                foreach (var key in attr.KeyMap)
                {
                    if (context.ArgumentsDictionary.TryGetValue(key, out var value))
                    {
                        var objectToString = _jsonSerializer.Serialize(value);
                        var objectMd5 = objectToString.ToMd5();
                        methodIdBuilder.AppendFormat(";i:{0};v:{1}", key, objectMd5);
                    }
                    else
                    {
                        methodIdBuilder.AppendFormat(";i:{0}", key);
                    }
                    index++;
                }
            }
        }
        else
        {
            var args = context.ArgumentsDictionary.ToImmutableArray();
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.Value == null)
                {
                    continue;
                }
                if (arg.Value.GetType().IsValueType)
                {
                    methodIdBuilder.AppendFormat(";i:{0};v:{1}", i, arg.ToString());
                }
                else
                {
                    var objectToString = _jsonSerializer.Serialize(arg.Value);
                    methodIdBuilder.AppendFormat(";i:{0};v:{1}", i, objectToString);
                }
            }
        }

        if (methodIdBuilder.Length <= 0)
        {
            methodIdBuilder.Append("unknown");
        }

        return string.Format(KeyFormat, context.Target.FullName, context.Method.Name, methodIdBuilder.ToString().ToMd5());
    }
}
