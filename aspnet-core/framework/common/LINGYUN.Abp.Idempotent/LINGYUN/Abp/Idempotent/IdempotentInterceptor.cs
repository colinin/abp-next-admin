using System.Threading.Tasks;
using Volo.Abp.Aspects;
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
        if (!ShouldIntercept(invocation))
        {
            await invocation.ProceedAsync();
            return;
        }

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

        await using var grant = await _idempotentChecker.IsGrantAsync(checkContext);
        if (!grant.Successed)
        {
            throw grant.Exception!;
        }
        await invocation.ProceedAsync();
    }

    protected virtual bool ShouldIntercept(IAbpMethodInvocation invocation)
    {
        if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, IdempotentCrossCuttingConcerns.Idempotent))
        {
            return false;
        }

        return true;
    }
}
