using System;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.IdentityServer.ApiResources;

public interface IApiResourceAppService : 
    ICrudAppService<
        ApiResourceDto,
        Guid,
        ApiResourceGetByPagedInputDto,
        ApiResourceCreateDto,
        ApiResourceUpdateDto>
{
}
