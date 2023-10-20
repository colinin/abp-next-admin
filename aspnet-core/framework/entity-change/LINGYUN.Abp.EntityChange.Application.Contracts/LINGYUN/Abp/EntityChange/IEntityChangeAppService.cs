using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.EntityChange;

public interface IEntityChangeAppService : IApplicationService
{
    Task<PagedResultDto<EntityChangeDto>> GetListAsync(EntityChangeGetListInput input);
}
