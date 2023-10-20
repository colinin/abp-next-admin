using LINGYUN.Linq.Dynamic.Queryable;

namespace LINGYUN.Abp.Dynamic.Queryable;

public class DynamicParamterDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string JavaScriptType { get; set; }
    public DynamicComparison[] AvailableComparator { get; set; }
    public ParamterOptionDto[] Options { get; set; }
    public DynamicParamterDto()
    {
        AvailableComparator = new DynamicComparison[0];
        Options = new ParamterOptionDto[0];
    }
}
