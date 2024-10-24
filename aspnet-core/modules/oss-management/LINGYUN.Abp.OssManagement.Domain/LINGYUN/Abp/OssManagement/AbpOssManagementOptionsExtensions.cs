using System.Collections.Generic;

namespace LINGYUN.Abp.OssManagement;

public static class AbpOssManagementOptionsExtensions
{
    public static void AddProcesser<TProcesserContributor>(
        this AbpOssManagementOptions options,
        TProcesserContributor contributor)
        where TProcesserContributor : IOssObjectProcesserContributor
    {
        options.Processers.InsertBefore((x) => x is NoneOssObjectProcesser, contributor);
    }
}
