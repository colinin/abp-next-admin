using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    public interface IIdentityResourceAppService : IApplicationService
    {
        Task<IdentityResourceDto> GetAsync(IdentityResourceGetByIdInputDto identityResourceGetById);

        Task<PagedResultDto<IdentityResourceDto>> GetAsync(IdentityResourceGetByPagedInputDto identityResourceGetByPaged);

        Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateDto identityResourceCreate);

        Task<IdentityResourceDto> UpdateAsync(IdentityResourceUpdateDto identityResourceUpdate);

        Task DeleteAsync(IdentityResourceGetByIdInputDto identityResourceGetById);

        Task<IdentityResourcePropertyDto> AddPropertyAsync(IdentityResourcePropertyCreateDto identityResourcePropertyCreate);

        Task DeletePropertyAsync(IdentityResourcePropertyGetByKeyDto identityResourcePropertyGetByKey);
    }
}
