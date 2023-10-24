namespace LINGYUN.Abp
{
    public interface IOptionsProvider<TValue>
        where TValue: class, new()
    {
        TValue Value { get; }
    }
}
