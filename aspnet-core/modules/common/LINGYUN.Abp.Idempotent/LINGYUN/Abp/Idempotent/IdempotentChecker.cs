using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;

namespace LINGYUN.Abp.Idempotent;

public class IdempotentChecker : IIdempotentChecker, ITransientDependency
{
    private readonly IAbpDistributedLock _distributedLock;
    private readonly AbpIdempotentOptions _idempotentOptions;
    private readonly IIdempotentDeniedHandler _idempotentDeniedHandler;

    public IdempotentChecker(
        IAbpDistributedLock distributedLock,
        IOptions<AbpIdempotentOptions> idempotentOptions, 
        IIdempotentDeniedHandler idempotentDeniedHandler)
    {
        _distributedLock = distributedLock;
        _idempotentOptions = idempotentOptions.Value;
        _idempotentDeniedHandler = idempotentDeniedHandler;
    }

    public async virtual Task CheckAsync(IdempotentCheckContext context)
    {
        if (!_idempotentOptions.IsEnabled)
        {
            return;
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

        await using var handle = await _distributedLock.TryAcquireAsync(context.IdempotentKey, TimeSpan.FromMilliseconds(methodLockTimeout));

        if (handle == null)
        {
            var deniedContext = new IdempotentDeniedContext(
                context.IdempotentKey,
                attr,
                context.Method,
                context.ArgumentsDictionary)
            {
                HttpStatusCode = _idempotentOptions.HttpStatusCode,
            };

            _idempotentDeniedHandler.Denied(deniedContext);
        }
    }
}
