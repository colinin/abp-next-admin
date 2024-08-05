using System;
using Volo.Abp;

namespace LINGYUN.Abp.Location;

public class LocationResolveException : AbpException, IBusinessException
{
    public LocationResolveException()
    {
    }

    public LocationResolveException(string message)
        : base(message)
    {
    }

    public LocationResolveException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
