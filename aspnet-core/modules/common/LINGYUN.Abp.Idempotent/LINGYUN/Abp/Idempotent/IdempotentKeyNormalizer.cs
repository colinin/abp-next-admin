using System;
using System.Reflection;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Idempotent;
public class IdempotentKeyNormalizer : IIdempotentKeyNormalizer, ITransientDependency
{
    private const string KeyFormat = "t:{0};m:{1};k:{}";

    private readonly IJsonSerializer _jsonSerializer;

    public IdempotentKeyNormalizer(
        IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

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
            if (typeof(ICreateAppService<,>).IsAssignableFrom(context.Target) &&
                "CreateAsync".Equals(context.Method.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                if (context.ArgumentsDictionary.TryGetValue("input", out var args))
                {
                    var objectToString = _jsonSerializer.Serialize(args);
                    var objectMd5 = objectToString.ToMd5();
                    methodIdBuilder.AppendFormat(";i:input;v:{0}", objectMd5);
                }
            }

            if (typeof(IUpdateAppService<,>).IsAssignableFrom(context.Target) &&
                "UpdateAsync".Equals(context.Method.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                if (context.ArgumentsDictionary.TryGetValue("id", out var idArgs) && idArgs != null)
                {
                    var idMd5 = idArgs.ToString().ToMd5();
                    methodIdBuilder.AppendFormat(";i:id;v:{0}", idMd5);
                }

                if (context.ArgumentsDictionary.TryGetValue("input", out var inputArgs) && inputArgs != null)
                {
                    var objectToString = _jsonSerializer.Serialize(inputArgs);
                    var objectMd5 = objectToString.ToMd5();
                    methodIdBuilder.AppendFormat(";i:input;v:{0}", objectMd5);
                }
            }

            if (typeof(IDeleteAppService<>).IsAssignableFrom(context.Target) &&
                "DeleteAsync".Equals(context.Method.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                if (context.ArgumentsDictionary.TryGetValue("id", out var idArgs) && idArgs != null)
                {
                    var idMd5 = idArgs.ToString().ToMd5();
                    methodIdBuilder.AppendFormat(";i:id;v:{0}", idMd5);
                }
            }
        }

        if (methodIdBuilder.Length <= 0)
        {
            methodIdBuilder.Append("unknown");
        }

        return string.Format(KeyFormat, context.Target.FullName, context.Method.Name, methodIdBuilder.ToString());
    }
}
