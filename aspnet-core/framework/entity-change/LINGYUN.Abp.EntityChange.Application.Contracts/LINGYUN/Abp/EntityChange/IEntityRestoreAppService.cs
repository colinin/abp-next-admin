using System.Threading.Tasks;

namespace LINGYUN.Abp.EntityChange;

public interface IEntityRestoreAppService : IEntityChangeAppService
{
    Task RestoreEntityAsync(RestoreEntityInput input);

    Task RestoreEntitesAsync(RestoreEntitiesInput input);
}
