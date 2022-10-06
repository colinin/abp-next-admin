using System;
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
}
