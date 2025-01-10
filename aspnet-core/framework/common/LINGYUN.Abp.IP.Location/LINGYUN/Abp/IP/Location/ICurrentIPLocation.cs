using System;

namespace LINGYUN.Abp.IP.Location;
public interface ICurrentIPLocation
{
    string? Country { get; }

    string? Province { get; }

    string? City { get; }

    string? Remarks { get; }

    IDisposable Change(IPLocation? location = null);
}
