using System.Threading.Tasks;

namespace LINGYUN.Abp.Idempotent;

public interface IIdempotentChecker
{
    Task CheckAsync(IdempotentCheckContext context);
}
