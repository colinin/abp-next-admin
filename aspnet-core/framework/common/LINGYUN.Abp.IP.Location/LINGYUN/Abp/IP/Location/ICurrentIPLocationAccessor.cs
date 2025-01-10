namespace LINGYUN.Abp.IP.Location;
public interface ICurrentIPLocationAccessor
{
    IPLocation? Current { get; set; }
}
