namespace LINGYUN.Abp.Location
{
    public interface ILocationResolveProvider
    {
        PositiveLocation GetPositiveLocation(string address);

        InverseLocation GetInverseLocation(double lat, double lng);
    }
}
