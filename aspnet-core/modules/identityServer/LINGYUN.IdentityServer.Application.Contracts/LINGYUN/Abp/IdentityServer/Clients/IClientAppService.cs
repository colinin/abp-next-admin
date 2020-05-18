using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public interface IClientAppService : IApplicationService
    {
        Task<ClientDto> GetAsync(ClientGetByIdInputDto clientGetById);

        Task<PagedResultDto<ClientDto>> GetAsync(ClientGetByPagedInputDto clientGetByPaged);

        Task<ClientDto> CreateAsync(ClientCreateDto clientCreate);

        Task<ClientDto> UpdateAsync(ClientUpdateInputDto clientUpdateInput);

        Task DeleteAsync(ClientGetByIdInputDto clientGetByIdInput);

        Task<ClientClaimDto> AddClaimAsync(ClientClaimCreateDto clientClaimCreate);

        Task<ClientClaimDto> UpdateClaimAsync(ClientClaimUpdateDto clientClaimUpdate);

        Task DeleteClaimAsync(ClientClaimGetByKeyInputDto clientClaimGetByKey);

        Task<ClientPropertyDto> AddPropertyAsync(ClientPropertyCreateDto clientPropertyCreate);

        Task<ClientPropertyDto> UpdatePropertyAsync(ClientPropertyUpdateDto clientPropertyUpdate);

        Task DeletePropertyAsync(ClientPropertyGetByKeyDto clientPropertyGetByKey);

        Task<ClientSecretDto> AddSecretAsync(ClientSecretCreateDto clientSecretCreate);

        Task<ClientSecretDto> UpdateSecretAsync(ClientSecretUpdateDto clientSecretUpdate);

        Task DeleteSecretAsync(ClientSecretGetByTypeDto clientSecretGetByType);
    }
}
