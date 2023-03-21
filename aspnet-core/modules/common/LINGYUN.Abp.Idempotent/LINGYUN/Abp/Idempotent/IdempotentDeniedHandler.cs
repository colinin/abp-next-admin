using System;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Idempotent;

public class IdempotentDeniedHandler : IIdempotentDeniedHandler, ISingletonDependency
{
    public virtual void Denied(IdempotentDeniedContext context)
    {
        var exception = new IdempotentDeniedException(context.IdempotentKey, IdempotentErrorCodes.IdempotentDenied)
                .WithData(nameof(IdempotentAttribute.IdempotentKey), context.IdempotentKey);

        if (context.Attribute != null && !string.IsNullOrWhiteSpace(context.Attribute.RedirectUrl))
        {
            var regex = new Regex("(?<={).+(?=})");
            if (regex.IsMatch(context.Attribute.RedirectUrl))
            {
                var matchValue = regex.Match(context.Attribute.RedirectUrl).Value;
                var replaceMatchKey = "{" + matchValue + "}";
                var redirectUrl = "";
                foreach (var arg in context.ArgumentsDictionary)
                {
                    if (arg.Value != null && string.Equals(arg.Key, matchValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        redirectUrl = context.Attribute.RedirectUrl!.Replace(replaceMatchKey, arg.Value.ToString());
                    }
                }

                if (redirectUrl.IsNullOrWhiteSpace())
                {
                    foreach (var arg in context.ArgumentsDictionary)
                    {
                        if (arg.Value == null)
                        {
                            continue;
                        }
                        var properties = arg.Value.GetType().GetProperties();
                        foreach (var propertyInfo in properties)
                        {
                            if (string.Equals(propertyInfo.Name, matchValue, StringComparison.InvariantCultureIgnoreCase))
                            {
                                var propValue = propertyInfo.GetValue(arg.Value);
                                if (propValue != null)
                                {
                                    redirectUrl = context.Attribute.RedirectUrl!.Replace(replaceMatchKey, propValue.ToString());
                                }
                            }
                        }
                    }
                }

                if (!redirectUrl.IsNullOrWhiteSpace())
                {
                    exception.WithData(nameof(IdempotentAttribute.RedirectUrl), redirectUrl);
                }
            }
        }

        throw exception;
    }
}
