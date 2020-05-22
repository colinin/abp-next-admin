using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public interface IApiResourceAppService : IApplicationService
    {
        Task<ApiResourceDto> GetAsync(ApiResourceGetByIdInputDto apiResourceGetById);

        Task<PagedResultDto<ApiResourceDto>> GetAsync(ApiResourceGetByPagedInputDto apiResourceGetByPaged);

        Task<ApiResourceDto> CreateAsync(ApiResourceCreateDto apiResourceCreate);

        Task<ApiResourceDto> UpdateAsync(ApiResourceUpdateDto apiResourceUpdate);

        Task DeleteAsync(ApiResourceGetByIdInputDto apiResourceGetById);

        Task<ApiSecretDto> AddSecretAsync(ApiSecretCreateDto apiSecretCreate);

        Task DeleteSecretAsync(ApiSecretGetByTypeInputDto apiSecretGetByType);

        Task<ApiScopeDto> AddScopeAsync(ApiScopeCreateDto apiScopeCreate);

        Task DeleteScopeAsync(ApiScopeGetByNameInputDto apiScopeGetByName);
    }
}
