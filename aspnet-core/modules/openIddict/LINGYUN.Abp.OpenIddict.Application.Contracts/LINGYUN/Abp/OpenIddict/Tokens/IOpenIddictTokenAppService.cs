using System;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OpenIddict.Tokens;

public interface IOpenIddictTokenAppService :
    IReadOnlyAppService<
        OpenIddictTokenDto,
        Guid,
        OpenIddictTokenGetListInput>,
    IDeleteAppService<Guid>
{
}
