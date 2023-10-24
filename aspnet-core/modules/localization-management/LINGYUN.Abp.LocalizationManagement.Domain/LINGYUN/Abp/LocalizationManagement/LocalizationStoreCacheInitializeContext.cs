using System;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationStoreCacheInitializeContext
{
    public IServiceProvider ServiceProvider { get; }
    public LocalizationStoreCacheInitializeContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
}
