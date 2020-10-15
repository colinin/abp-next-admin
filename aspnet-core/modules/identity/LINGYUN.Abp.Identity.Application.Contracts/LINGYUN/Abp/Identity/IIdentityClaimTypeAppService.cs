using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Identity
{
    public interface IIdentityClaimTypeAppService : 
        ICrudAppService<
            IdentityClaimTypeDto,
            Guid,
            IdentityClaimTypeGetByPagedDto,
            IdentityClaimTypeCreateDto,
            IdentityClaimTypeUpdateDto>
    {
        Task<ListResultDto<IdentityClaimTypeDto>> GetAllListAsync();
    }
}
