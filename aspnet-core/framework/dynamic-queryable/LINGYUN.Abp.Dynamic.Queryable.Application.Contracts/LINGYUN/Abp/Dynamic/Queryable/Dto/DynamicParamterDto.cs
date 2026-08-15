using LINGYUN.Linq.Dynamic.Queryable;

namespace LINGYUN.Abp.Dynamic.Queryable;

public class DynamicParamterDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string JavaScriptType { get; set; } = default!;
    public DynamicComparison[] AvailableComparator { get; set; }
    public ParamterOptionDto[] Options { get; set; }
    public DynamicParamterDto()
    {
        AvailableComparator = new DynamicComparison[0];
        Options = new ParamterOptionDto[0];
    }
}
