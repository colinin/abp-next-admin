using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.IdentityServer.Clients;

public interface IClientAppService : 
    ICrudAppService<
        ClientDto,
        Guid,
        ClientGetByPagedDto,
        ClientCreateDto,
        ClientUpdateDto>
{
    Task<ClientDto> CloneAsync(Guid id, ClientCloneDto input);

    Task<ListResultDto<string>> GetAssignableApiResourcesAsync();

    Task<ListResultDto<string>> GetAssignableIdentityResourcesAsync();

    Task<ListResultDto<string>> GetAllDistinctAllowedCorsOriginsAsync();
}
