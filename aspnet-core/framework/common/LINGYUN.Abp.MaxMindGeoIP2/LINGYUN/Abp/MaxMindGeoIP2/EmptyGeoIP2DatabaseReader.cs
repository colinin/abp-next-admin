using MaxMind.Db;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
using System.Net;

namespace LINGYUN.Abp.MaxMindGeoIP2;

/// <summary>
/// 空的 GeoIP2 数据库解读器
/// </summary>
/// <remarks>
/// 未提供(或找不到)离线库文件时使用: 相当于解读了一份空内容,
/// 所有查询都返回"无数据"——<c>TryXxx</c> 返回 <c>false</c>、<c>Xxx</c> 抛出
/// <see cref="AddressNotFoundException"/>, 与真实解读器未命中时的行为一致, 不影响业务流程
/// </remarks>
public class EmptyGeoIP2DatabaseReader : IGeoIP2DatabaseReader
{
    /// <summary>
    /// 空解读器不提供数据库元数据
    /// </summary>
    public virtual Metadata Metadata => null!;

    public virtual CityResponse City(IPAddress ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual CityResponse City(string ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual CountryResponse Country(IPAddress ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual CountryResponse Country(string ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual AnonymousIPResponse AnonymousIP(IPAddress ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual AnonymousIPResponse AnonymousIP(string ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual AnonymousPlusResponse AnonymousPlus(IPAddress ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual AnonymousPlusResponse AnonymousPlus(string ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual AsnResponse Asn(IPAddress ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual AsnResponse Asn(string ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual ConnectionTypeResponse ConnectionType(IPAddress ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual ConnectionTypeResponse ConnectionType(string ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual DomainResponse Domain(IPAddress ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual DomainResponse Domain(string ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual EnterpriseResponse Enterprise(IPAddress ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual EnterpriseResponse Enterprise(string ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual IspResponse Isp(IPAddress ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual IspResponse Isp(string ipAddress) => throw CreateNotFoundException(ipAddress);

    public virtual bool TryCity(IPAddress ipAddress, out CityResponse response) => TryGet(out response);

    public virtual bool TryCity(string ipAddress, out CityResponse response) => TryGet(out response);

    public virtual bool TryCountry(IPAddress ipAddress, out CountryResponse response) => TryGet(out response);

    public virtual bool TryCountry(string ipAddress, out CountryResponse response) => TryGet(out response);

    public virtual bool TryAnonymousIP(IPAddress ipAddress, out AnonymousIPResponse response) => TryGet(out response);

    public virtual bool TryAnonymousIP(string ipAddress, out AnonymousIPResponse response) => TryGet(out response);

    public virtual bool TryAnonymousPlus(IPAddress ipAddress, out AnonymousPlusResponse response) => TryGet(out response);

    public virtual bool TryAnonymousPlus(string ipAddress, out AnonymousPlusResponse response) => TryGet(out response);

    public virtual bool TryAsn(IPAddress ipAddress, out AsnResponse response) => TryGet(out response);

    public virtual bool TryAsn(string ipAddress, out AsnResponse response) => TryGet(out response);

    public virtual bool TryConnectionType(IPAddress ipAddress, out ConnectionTypeResponse response) => TryGet(out response);

    public virtual bool TryConnectionType(string ipAddress, out ConnectionTypeResponse response) => TryGet(out response);

    public virtual bool TryDomain(IPAddress ipAddress, out DomainResponse response) => TryGet(out response);

    public virtual bool TryDomain(string ipAddress, out DomainResponse response) => TryGet(out response);

    public virtual bool TryEnterprise(IPAddress ipAddress, out EnterpriseResponse response) => TryGet(out response);

    public virtual bool TryEnterprise(string ipAddress, out EnterpriseResponse response) => TryGet(out response);

    public virtual bool TryIsp(IPAddress ipAddress, out IspResponse response) => TryGet(out response);

    public virtual bool TryIsp(string ipAddress, out IspResponse response) => TryGet(out response);

    /// <summary>
    /// 所有查询都没有数据
    /// </summary>
    protected virtual bool TryGet<TResponse>(out TResponse response)
        where TResponse : class
    {
        response = null!;

        return false;
    }

    protected virtual AddressNotFoundException CreateNotFoundException(object? ipAddress)
    {
        return new AddressNotFoundException($"The MaxMind GeoIP2 database is empty, no data for {ipAddress}.");
    }
}
