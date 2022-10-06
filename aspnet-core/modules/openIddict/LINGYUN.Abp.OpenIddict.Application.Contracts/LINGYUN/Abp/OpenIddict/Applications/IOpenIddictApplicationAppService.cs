using System;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OpenIddict.Applications;

public interface IOpenIddictApplicationAppService : 
    ICrudAppService<
        OpenIddictApplicationDto,
        Guid,
        OpenIddictApplicationGetListInput,
        OpenIddictApplicationCreateDto,
        OpenIddictApplicationUpdateDto>
{
}
