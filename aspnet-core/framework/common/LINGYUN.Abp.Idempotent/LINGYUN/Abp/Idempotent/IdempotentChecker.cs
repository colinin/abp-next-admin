using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;

namespace LINGYUN.Abp.Idempotent;

public class IdempotentChecker : IIdempotentChecker, ITransientDependency
{
    private readonly IAbpDistributedLock _distributedLock;
    private readonly AbpIdempotentOptions _idempotentOptions;

    public IdempotentChecker(
        IAbpDistributedLock distributedLock,
        IOptions<AbpIdempotentOptions> idempotentOptions)
    {
        _distributedLock = distributedLock;
        _idempotentOptions = idempotentOptions.Value;
    }

    [IgnoreIdempotent]
    public async virtual Task<IdempotentGrantResult> IsGrantAsync(IdempotentCheckContext context)
    {
        if (!_idempotentOptions.IsEnabled)
        {
            return new IdempotentGrantResult(context.IdempotentKey);
        }

        if (context.Method.IsDefined(typeof(IgnoreIdempotentAttribute), true))
        {
            return new IdempotentGrantResult(context.IdempotentKey);
        }

        var attr = context.Method.GetCustomAttribute<IdempotentAttribute>();

        var methodLockTimeout = _idempotentOptions.DefaultTimeout;

        if (attr != null)
        {
            if (attr.Timeout.HasValue)
            {
                methodLockTimeout = attr.Timeout.Value;
            }
        }

        var handle = await _distributedLock.TryAcquireAsync(context.IdempotentKey, TimeSpan.FromMilliseconds(methodLockTimeout));

        if (handle == null)
        {
            var exception = new BusinessException(IdempotentErrorCodes.IdempotentDenied);

            if (attr != null && !string.IsNullOrWhiteSpace(attr.RedirectUrl))
            {
                var regex = new Regex("(?<={).+(?=})");
                if (regex.IsMatch(attr.RedirectUrl))
                {
                    var matchValue = regex.Match(attr.RedirectUrl).Value;
                    var replaceMatchKey = "{" + matchValue + "}";
                    var redirectUrl = "";
                    foreach (var arg in context.ArgumentsDictionary)
                    {
                        if (arg.Value != null && string.Equals(arg.Key, matchValue, StringComparison.InvariantCultureIgnoreCase))
                        {
                            redirectUrl = attr.RedirectUrl!.Replace(replaceMatchKey, arg.Value.ToString());
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
                                        redirectUrl = attr.RedirectUrl!.Replace(replaceMatchKey, propValue.ToString());
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

            return new IdempotentGrantResult(context.IdempotentKey, exception);
        }

        return new IdempotentGrantResult(context.IdempotentKey, disposable: handle);
    }

}
