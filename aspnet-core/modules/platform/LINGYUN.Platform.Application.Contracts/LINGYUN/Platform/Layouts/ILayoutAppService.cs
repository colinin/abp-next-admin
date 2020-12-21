using System;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Layouts
{
    public interface ILayoutAppService :
        ICrudAppService<
            LayoutDto,
            Guid,
            GetLayoutListInput,
            LayoutCreateDto,
            LayoutUpdateDto>
    {

    }
}
