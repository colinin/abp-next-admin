using System;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OpenIddict.Authorizations;

public interface IOpenIddictAuthorizationAppService :
    IReadOnlyAppService<
        OpenIddictAuthorizationDto,
        Guid,
        OpenIddictAuthorizationGetListInput>,
    IDeleteAppService<Guid>
{
}
