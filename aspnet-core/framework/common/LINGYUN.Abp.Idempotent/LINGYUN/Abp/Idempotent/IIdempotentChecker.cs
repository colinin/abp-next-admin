using System.Threading.Tasks;

namespace LINGYUN.Abp.Idempotent;

public interface IIdempotentChecker
{
    Task<IdempotentGrantResult> IsGrantAsync(IdempotentCheckContext context);
}
