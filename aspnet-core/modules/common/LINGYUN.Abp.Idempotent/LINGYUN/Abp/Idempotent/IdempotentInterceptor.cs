using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.Idempotent;

public class IdempotentInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IIdempotentChecker _idempotentChecker;
    private readonly IIdempotentKeyNormalizer _idempotentKeyNormalizer;

    public IdempotentInterceptor(
        IIdempotentChecker idempotentChecker,
        IIdempotentKeyNormalizer idempotentKeyNormalizer)
    {
        _idempotentChecker = idempotentChecker;
        _idempotentKeyNormalizer = idempotentKeyNormalizer;
    }

    [IgnoreIdempotent]
    public async override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        var targetType = ProxyHelper.GetUnProxiedType(invocation.TargetObject);

        var keyNormalizerContext = new IdempotentKeyNormalizerContext(
            targetType,
            invocation.Method,
            invocation.ArgumentsDictionary);

        var idempotentKey = _idempotentKeyNormalizer.NormalizeKey(keyNormalizerContext);

        var checkContext = new IdempotentCheckContext(
            targetType,
            invocation.Method,
            idempotentKey,
            invocation.ArgumentsDictionary);

        await _idempotentChecker.CheckAsync(checkContext);

        await invocation.ProceedAsync();
    }
}
