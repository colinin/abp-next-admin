namespace LINGYUN.Abp.DataProtection
{
    /// <summary>
    /// 实现接口
    /// 数据访问保护
    /// </summary>
    public interface IDataProtection
    {
        string Owner { get; }
    }
}
