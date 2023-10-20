using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.Idempotent;

public interface IIdempotentKeyNormalizer
{
    string NormalizeKey(IdempotentKeyNormalizerContext context);
}
