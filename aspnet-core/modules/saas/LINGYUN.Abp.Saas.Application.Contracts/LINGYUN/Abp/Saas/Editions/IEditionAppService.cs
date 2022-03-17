using System;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Saas.Editions;

public interface IEditionAppService :
    ICrudAppService<
        EditionDto,
        Guid,
        EditionGetListInput,
        EditionCreateDto,
        EditionUpdateDto>
{
}
