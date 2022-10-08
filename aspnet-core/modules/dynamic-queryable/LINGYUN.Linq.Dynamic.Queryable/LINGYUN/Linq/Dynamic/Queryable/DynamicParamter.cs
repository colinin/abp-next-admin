namespace LINGYUN.Linq.Dynamic.Queryable;

public class DynamicParamter
{
    public string Filed { get; set; }

    public DynamicLogic Logic { get; set; } = DynamicLogic.And;

    public DynamicComparison Comparison { get; set; } = DynamicComparison.Equal;

    public object Value { get; set; }
    public string Type { get; set; }
}
