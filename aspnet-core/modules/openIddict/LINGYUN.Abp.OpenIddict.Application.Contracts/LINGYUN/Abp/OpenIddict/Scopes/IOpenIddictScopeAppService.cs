using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OpenIddict.Scopes;

public interface IOpenIddictScopeAppService :
    ICrudAppService<
        OpenIddictScopeDto,
        Guid,
        OpenIddictScopeGetListInput,
        OpenIddictScopeCreateDto,
        OpenIddictScopeUpdateDto>
{
    Task<ListResultDto<OpenIddictScopeDto>> GetAssignableScopesAsync();
}
