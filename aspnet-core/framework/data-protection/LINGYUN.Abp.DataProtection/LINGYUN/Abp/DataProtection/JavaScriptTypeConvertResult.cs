namespace LINGYUN.Abp.DataProtection;

public class JavaScriptTypeConvertResult
{
    public string Type { get; }
    public DataAccessFilterOperate[] AllowOperates { get; }
    public JavaScriptTypeConvertResult(string type, DataAccessFilterOperate[] allowOperates)
    {
        Type = type;
        AllowOperates = allowOperates;
    }
}
