namespace LINGYUN.Abp.IP.Location;
public class IPLocation
{
    public string? Country { get; }
    public string? Province { get;}
    public string? City { get; }
    public string? Remarks { get; set; }
    public IPLocation(
        string? country = null,
        string? province = null,
        string? city = null, 
        string? remarks = null)
    {
        Country = country;
        Province = province;
        City = city;
        Remarks = remarks;
    }
}
