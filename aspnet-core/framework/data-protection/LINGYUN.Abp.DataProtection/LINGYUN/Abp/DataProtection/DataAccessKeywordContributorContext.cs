using System;

namespace LINGYUN.Abp.DataProtection;
public class DataAccessKeywordContributorContext
{
    public IServiceProvider ServiceProvider { get; }
    public Type ConversionType { get; }
    public DataAccessKeywordContributorContext(
        IServiceProvider serviceProvider,
        Type conversionType)
    {
        ServiceProvider = serviceProvider;
        ConversionType = conversionType;
    }
}
