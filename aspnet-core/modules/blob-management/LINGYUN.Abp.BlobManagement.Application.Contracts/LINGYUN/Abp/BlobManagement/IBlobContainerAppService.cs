using LINGYUN.Abp.BlobManagement.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.BlobManagement;

public interface IBlobContainerAppService : IApplicationService
{
    Task<BlobContainerDto> CreateAsync(BlobContainerCreateDto input);

    Task DeleteAsync(Guid id);

    Task<BlobContainerDto> GetAsync(Guid id);

    Task<PagedResultDto<BlobContainerDto>> GetListAsync(BlobContainerGetPagedListInput input);
}
