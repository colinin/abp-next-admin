using LINGYUN.Platform.Routes;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Layouts;

public class GetLayoutListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    [DynamicStringLength(typeof(LayoutConsts), nameof(LayoutConsts.MaxFrameworkLength))]
    public string Framework { get; set; }
}
