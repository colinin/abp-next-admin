using System;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Portal;

public interface IEnterpriseAppService :
    ICrudAppService<
        EnterpriseDto,
        Guid,
        EnterpriseGetListInput,
        EnterpriseCreateDto,
        EnterpriseUpdateDto>
{
}
