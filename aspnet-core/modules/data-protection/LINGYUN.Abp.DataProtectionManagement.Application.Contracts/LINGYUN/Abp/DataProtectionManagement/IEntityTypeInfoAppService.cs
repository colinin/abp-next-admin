using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.DataProtectionManagement;
public interface IEntityTypeInfoAppService : IApplicationService
{
    Task<EntityTypeInfoDto> GetAsync(Guid id);

    Task<PagedResultDto<EntityTypeInfoDto>> GetListAsync(GetEntityTypeInfoListInput input);
}
